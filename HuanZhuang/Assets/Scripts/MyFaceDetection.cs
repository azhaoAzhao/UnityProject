using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using OpenCVForUnity.ObjdetectModule;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.ImgcodecsModule;
using UnityEngine.UI;
using OpenCVForUnity.UnityUtils.Helper;
using Rect = UnityEngine.Rect;

namespace OpenCVForUnityExample
{
    /// <summary>
    /// Face Detection Example
    /// An example of human face detection using the CascadeClassifier class.
    /// http://docs.opencv.org/3.2.0/db/d28/tutorial_cascade_classifier.html
    /// </summary>
    public class MyFaceDetection : Singleton<MyFaceDetection>
    {
        public static MyFaceDetection instance;
        CascadeClassifier cascade;
        public RawImage icon;

#if UNITY_WEBGL && !UNITY_EDITOR
        IEnumerator getFilePath_Coroutine;
#endif
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        // Use this for initialization
        void Start()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            getFilePath_Coroutine = Utils.getFilePathAsync("haarcascade_frontalface_alt.xml", 
            (result) => {
                getFilePath_Coroutine = null;
 
                cascade = new CascadeClassifier ();
                cascade.load(result);
                if (cascade.empty ()) {
                    Debug.LogError ("cascade file is not loaded. Please copy from “OpenCVForUnity/StreamingAssets/” to “Assets/StreamingAssets/” folder. ");
                }
           
                Run ();
            }, 
            (result, progress) => {
                Debug.Log ("getFilePathAsync() progress : " + result + " " + Mathf.CeilToInt (progress * 100) + "%");
            });
            StartCoroutine (getFilePath_Coroutine);
#else
            //cascade = new CascadeClassifier (Utils.getFilePath ("lbpcascade_frontalface.xml"));
            cascade = new CascadeClassifier();
            cascade.load(Utils.getFilePath("haarcascade_frontalface_alt.xml"));
#if !UNITY_WSA_10_0
            if (cascade.empty())
            {
                Debug.LogError(
                    "cascade file is not loaded. Please copy from “OpenCVForUnity/StreamingAssets/” to “Assets/StreamingAssets/” folder. ");
            }
#endif
            // Run ();
#endif
        }

        public bool CheckFace(Texture2D texture2D)
        {
            //加载图片
            Texture2D imgTexture = texture2D;
            //转成Mat
            Mat imgMat = new Mat(imgTexture.height, imgTexture.width, CvType.CV_8UC4);
            Utils.texture2DToMat(imgTexture, imgMat);
            //转为灰度图像
            Mat grayMat = new Mat();
            Imgproc.cvtColor(imgMat, grayMat, Imgproc.COLOR_RGBA2GRAY);
            Imgproc.equalizeHist(grayMat, grayMat);
            //检测图像中的所有脸
            MatOfRect faces = new MatOfRect();
            if (cascade != null)
                cascade.detectMultiScale(grayMat, faces, 1.1, 3, 2,
                    new Size(50, 50), new Size());
            OpenCVForUnity.CoreModule.Rect[] rects = faces.toArray();
            return faces.toArray().Length > 0;
        }

        public bool Run(Texture2D texture2D)
        {
            //加载图片
            Texture2D imgTexture = texture2D;
            //转成Mat
            Mat imgMat = new Mat(imgTexture.height, imgTexture.width, CvType.CV_8UC4);
            Utils.texture2DToMat(imgTexture, imgMat);
            //转为灰度图像
            Mat grayMat = new Mat();
            Imgproc.cvtColor(imgMat, grayMat, Imgproc.COLOR_RGBA2GRAY);
            Imgproc.equalizeHist(grayMat, grayMat);
            //检测图像中的所有脸
            MatOfRect faces = new MatOfRect();
            if (cascade != null)
                cascade.detectMultiScale(grayMat, faces, 1.1, 3, 2,
                    new Size(50, 50), new Size());
            OpenCVForUnity.CoreModule.Rect[] rects = faces.toArray();
            if (faces.toArray().Length == 1)
            {
                Debug.Log("识别到人脸" + faces.toArray().Length);
                //**********显示人脸区域
                for (int i = 0; i < rects.Length; i++)
                {
                    Imgproc.rectangle(imgMat, new Point(rects[i].x, rects[i].y),
                        new Point(rects[i].x + rects[i].width, rects[i].y + rects[i].height), new Scalar(255, 0, 0, 255), 2);
                }

                Texture2D texture = new Texture2D(imgMat.cols(), imgMat.rows(), TextureFormat.RGBA32, false);
                Utils.matToTexture2D(imgMat, texture);
                icon.texture = texture;


                //截取人脸部分
                InterceptImg(rects[0], texture2D);
                return true;
            }
            else
            {
                Debug.Log("未识别到人脸");
                return false;
            }
        }

        /// <summary>
        /// Raises the destroy event.
        /// </summary>
        void OnDestroy()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            if (getFilePath_Coroutine != null) {
                StopCoroutine (getFilePath_Coroutine);
                ((IDisposable)getFilePath_Coroutine).Dispose ();
            }
#endif
        }

        /// <summary>
        /// 截取图片
        /// </summary>
        /// <param name="rect">要截取的区域</param>
        void InterceptImg(OpenCVForUnity.CoreModule.Rect rect, Texture2D imgTexture)
        {
            //加载要截取的图片
            // Texture2D imgTexture = Resources.Load("photo") as Texture2D;
            Mat cameraMat = new Mat(imgTexture.height, imgTexture.width, CvType.CV_8UC4);
            Utils.texture2DToMat(imgTexture, cameraMat);
            //截取需要的部分 rect为上面检测的人脸区域
            Mat croppedImage = new Mat(cameraMat, rect);
            //色彩转换 如果不加 图片颜色会不对
            Imgproc.cvtColor(croppedImage, croppedImage, Imgproc.COLOR_RGBA2BGRA);
            //这里截取到的图片为倒的 使用这个方法翻转一下
            Core.flip(croppedImage, croppedImage, 1);
            string path = Application.streamingAssetsPath + "/Cap/" + Time.time + ".png";
            //保存到Assets目录下
            Imgcodecs.imwrite(path, croppedImage);


            string imgtype = "*.BMP|*.JPG|*.GIF|*.PNG";
            string[] ImageType = imgtype.Split('|');
            string currFIleName = System.IO.Path.GetFileNameWithoutExtension(path); //当前扩展名称

            for (int i = 0; i < ImageType.Length; i++)
            {
                //获取Application.dataPath文件夹下所有的图片路径  
                string[] dirs = Directory.GetFiles((Application.streamingAssetsPath + "/Cap/"), ImageType[i]);
                for (int j = 0; j < dirs.Length; j++)
                {
                    string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(dirs[j]);
                    if (fileNameWithoutExtension == currFIleName)
                    {
                        Texture2D tx = new Texture2D(503, 503);
                        tx.LoadImage(getImageByte(dirs[j]));
                        GameManager.Instance.CurrTexture2D = tx;
                        return;
                    }
                }
            }
        }

        /// <summary>  
        /// 根据图片路径返回图片的字节流byte[]  
        /// </summary>  
        /// <param name="imagePath">图片路径</param>  
        /// <returns>返回的字节流</returns>  
        private static byte[] getImageByte(string imagePath)
        {
            FileStream files = new FileStream(imagePath, FileMode.Open);
            byte[] imgByte = new byte[files.Length];
            files.Read(imgByte, 0, imgByte.Length);
            files.Close();
            return imgByte;
        }
    }
}