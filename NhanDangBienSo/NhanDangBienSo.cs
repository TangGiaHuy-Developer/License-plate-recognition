using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using ZedGraph;
using Tesseract;

namespace NhanDangBienSo
{
    public partial class NhanDangBienSo: Form
    {
        public NhanDangBienSo()
        {
            InitializeComponent();
        }
        private OpenFileDialog openFileDialog;
        private string folderImage = @"\danhSachHinhAnh_HuanLuyen";
        private string saveFolder = Application.StartupPath + @"\Characters";
        private string tessDataPath = @"C:\Program Files\Tesseract-OCR\tessdata";
        private Image<Bgr, byte> imageBgr;
        private Image<Gray, byte> imageGray;
        private Image<Gray, byte> imageSmoothBlur;
        private Image<Gray, byte> imageBinary;
        private Image<Bgr, byte> imagePlate;
        private Image<Gray, byte> imagePlateGray;
        private Image<Gray, byte> imagePlateSmoothBlur;
        private Image<Gray, byte> imagePlateBinary;
        private List<Image<Bgr, byte>> imageCharacter_Unsort;
        private List<Image<Bgr, byte>> imageCharacter_Sort;
        private List<int> x_axisPosition;
        private List<int> y_axisPosition;
        //Tesseract 
        private List<Image<Bgr, byte>> imageCityCode;
        private List<Image<Bgr, byte>> imageLetter_1;
        private List<Image<Bgr, byte>> imageLetter_2;
        private List<Image<Bgr, byte>> imageNumberGroup;


        private void NhanDangBienSo_Load(object sender, EventArgs e)
        {
            pictureBox_Screen.Width = this.Width;
            pictureBox_Screen.Height = this.Height;
            pictureBox_Screen.Location = new Point(0, thanhChucNang.Height);
            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Application.StartupPath + folderImage;
        }

        private void NhanDangBienSo_Resize(object sender, EventArgs e)
        {
            NhanDangBienSo_Load(sender, e);
        }

        private void chucNang_ThemAnh_Click(object sender, EventArgs e)
        {
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                imageBgr = new Image<Bgr, byte>(openFileDialog.FileName);
                imageBgr.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
                pictureBox_Screen.Image = imageBgr.Bitmap;
            }
        }

        private void chinhSua_AnhXam_Click(object sender, EventArgs e)
        {
            if (imageBgr == null)
            {
                MessageBox.Show("Bạn chưa có ảnh để chỉnh sửa!","THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                imageBgr.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
                imageGray = imageBgr.Convert<Gray, byte>();
                pictureBox_Screen.Image = imageGray.Bitmap;
            }               
        }

        private void chinhSua_NhiPhan_Click(object sender, EventArgs e)
        {
            if (imageBgr == null)
            {
                MessageBox.Show("Bạn chưa có ảnh để chỉnh sửa!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                imageBgr.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
                imageGray = imageBgr.Convert<Gray, byte>();
                imageSmoothBlur = imageGray.SmoothBlur(5, 5);
                imageBinary = imageSmoothBlur.ThresholdBinary(new Gray(150),new Gray(255));
                pictureBox_Screen.Image = imageBinary.Bitmap;
            }
        }

        private void chinhSua_LamMo_Click(object sender, EventArgs e)
        {
            if (imageBgr == null)
            {
                MessageBox.Show("Bạn chưa có ảnh để chỉnh sửa!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                imageBgr.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
                imageGray = imageBgr.Convert<Gray, byte>();
                imageSmoothBlur = imageGray.SmoothBlur(5, 5);
                pictureBox_Screen.Image = imageSmoothBlur.Bitmap;
            }
        }

     

        private void chinhSua_VeDuongVien_Click(object sender, EventArgs e)
        {
            if (imageBgr == null)
            {
                MessageBox.Show("Bạn chưa có ảnh để chỉnh sửa!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                imageBgr.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
                imageGray = imageBgr.Convert<Gray, byte>();
                imageSmoothBlur = imageGray.SmoothBlur(5, 5);
                imageBinary = imageSmoothBlur.ThresholdBinary(new Gray(150), new Gray(255));
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();                
                CvInvoke.FindContours(imageBinary, contours, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImage = imageBgr.CopyBlank();
                for (int i = 0; i < contours.Size; i++)
                    CvInvoke.Polylines(drawImage, contours[i], true, new MCvScalar(0, 255, 0), 2);
                pictureBox_Screen.Image = drawImage.Bitmap;                
            }         
        }

        private void chinhSua_PhanVung_Click(object sender, EventArgs e)
        {
            if (imageBgr == null)
            {
                MessageBox.Show("Bạn chưa có ảnh để chỉnh sửa!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                imageBgr.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
                imageGray = imageBgr.Convert<Gray, byte>();
                imageSmoothBlur = imageGray.SmoothBlur(5, 5);
                imageBinary = imageSmoothBlur.ThresholdBinary(new Gray(150), new Gray(255));
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imageBinary, contours, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImage = imageBgr.CopyBlank();
                for (int i = 0; i < contours.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contours[i]);
                    double area = rectangle.Width * rectangle.Height;
                    CvInvoke.Polylines(drawImage, contours[i], true, new MCvScalar(0, 255, 0), 2);
                    if (area > 0 && area < 50000)
                        CvInvoke.Polylines(drawImage, contours[i], true, new MCvScalar(255, 0, 0), 2);
                    if (area > 50000 && area < 250000)
                        CvInvoke.Polylines(drawImage, contours[i], true, new MCvScalar(255, 0, 255), 2);
                    if (area > 250000 && area < 300000)
                        CvInvoke.Polylines(drawImage, contours[i], true, new MCvScalar(0, 255, 255), 2);
                    if (area > 300000 && area < 400000)
                        CvInvoke.Polylines(drawImage, contours[i], true, new MCvScalar(0, 0, 255), 2);
                    if (area > 400000)
                        CvInvoke.Polylines(drawImage, contours[i], true, new MCvScalar(255, 255, 0), 2);
                }
                pictureBox_Screen.Image = drawImage.Bitmap;                
            }
        }

        private void chinhSua_LocVungPhamVi_Click(object sender, EventArgs e)
        {
            if (imageBgr == null)
            {
                MessageBox.Show("Bạn chưa có ảnh để chỉnh sửa!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                imageBgr.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
                imageGray = imageBgr.Convert<Gray, byte>();
                imageSmoothBlur = imageGray.SmoothBlur(5, 5);
                imageBinary = imageSmoothBlur.ThresholdBinary(new Gray(150), new Gray(255));
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imageBinary, contours, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImage = imageBgr.CopyBlank();
                for (int i = 0; i < contours.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contours[i]);
                    double area = rectangle.Width * rectangle.Height;
                    if (area > 50000 && area < 250000)
                        CvInvoke.Polylines(drawImage, contours[i], true, new MCvScalar(255, 0, 255), 2);
                }
                pictureBox_Screen.Image = drawImage.Bitmap;                
            }
        }

        private void chinhSua_LocTiLe_Click(object sender, EventArgs e)
        {
            if (imageBgr == null)
            {
                MessageBox.Show("Bạn chưa có ảnh để chỉnh sửa!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                imageBgr.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
                imageGray = imageBgr.Convert<Gray, byte>();
                imageSmoothBlur = imageGray.SmoothBlur(5, 5);
                imageBinary = imageSmoothBlur.ThresholdBinary(new Gray(150), new Gray(255));
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();                
                CvInvoke.FindContours(imageBinary, contours, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImage = imageBgr.CopyBlank();
                for (int i = 0; i < contours.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contours[i]);
                    double area = rectangle.Width * rectangle.Height;
                    double aspectRatio = (double)rectangle.Width / rectangle.Height; 
                    if (area > 50000 && area < 250000)
                    {
                        CvInvoke.Polylines(drawImage, contours[i], true, new MCvScalar(255, 0, 255), 2);
                        if (rectangle.Width > rectangle.Height && aspectRatio > 1 && aspectRatio < 2)                        
                            CvInvoke.Rectangle(drawImage, rectangle, new MCvScalar(0, 255, 0), 2);                       
                    }
                }
                pictureBox_Screen.Image = drawImage.Bitmap;
            }
        }

        private void chinhSua_VeVungBienSo_Click(object sender, EventArgs e)
        {
            if (imageBgr == null)
            {
                MessageBox.Show("Bạn chưa có ảnh để chỉnh sửa!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                imageBgr.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
                imageGray = imageBgr.Convert<Gray, byte>();
                imageSmoothBlur = imageGray.SmoothBlur(5, 5);
                imageBinary = imageSmoothBlur.ThresholdBinary(new Gray(150), new Gray(255));
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();             
                CvInvoke.FindContours(imageBinary, contours, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImage = imageBgr.Clone();
                for (int i = 0; i < contours.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contours[i]);
                    double area = rectangle.Width * rectangle.Height;
                    double aspectRatio = (double)rectangle.Width / rectangle.Height;
                    if (area > 50000 && area < 250000)                    
                        if (rectangle.Width > rectangle.Height && aspectRatio > 1 && aspectRatio < 2)                        
                            CvInvoke.Rectangle(drawImage, rectangle, new MCvScalar(0, 255, 0), 5);      
                }
                pictureBox_Screen.Image = drawImage.Bitmap;                
            }
        }

        private void chinhSua_CatVungBienSo_Click(object sender, EventArgs e)
        {
            if (imageBgr == null)
            {
                MessageBox.Show("Bạn chưa có ảnh để chỉnh sửa!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                imageBgr.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
                imageGray = imageBgr.Convert<Gray, byte>();
                imageSmoothBlur = imageGray.SmoothBlur(5, 5);
                imageBinary = imageSmoothBlur.ThresholdBinary(new Gray(150), new Gray(255));
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imageBinary, contours, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImage = imageBgr.Clone();
                for (int i = 0; i < contours.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contours[i]);
                    double area = rectangle.Width * rectangle.Height;
                    double aspectRatio = (double)rectangle.Width / rectangle.Height;
                    if (area > 50000 && area < 250000)
                        if (rectangle.Width > rectangle.Height && aspectRatio > 1 && aspectRatio < 2)
                            imagePlate = imageBgr.Copy(rectangle);
                }
                pictureBox_Screen.Image = imagePlate.Bitmap;
            }
        }

        private void chinhSua_bienSoMauXam_Click(object sender, EventArgs e)
        {
            if (imageBgr == null)
            {
                MessageBox.Show("Bạn chưa có ảnh để chỉnh sửa!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                imageBgr.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
                imageGray = imageBgr.Convert<Gray, byte>();
                imageSmoothBlur = imageGray.SmoothBlur(5, 5);
                imageBinary = imageSmoothBlur.ThresholdBinary(new Gray(150), new Gray(255));
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imageBinary, contours, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImage = imageBgr.Clone();
                for (int i = 0; i < contours.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contours[i]);
                    double area = rectangle.Width * rectangle.Height;
                    double aspectRatio = (double)rectangle.Width / rectangle.Height;
                    if (area > 50000 && area < 250000)
                        if (rectangle.Width > rectangle.Height && aspectRatio > 1 && aspectRatio < 2)
                            imagePlate = imageBgr.Copy(rectangle);
                }
                imagePlateGray = imagePlate.Convert<Gray, byte>();
                pictureBox_Screen.Image = imagePlateGray.Bitmap;
            }
        }

        private void chinhSua_bienSoLamMo_Click(object sender, EventArgs e)
        {
            if (imageBgr == null)
            {
                MessageBox.Show("Bạn chưa có ảnh để chỉnh sửa!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                imageBgr.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
                imageGray = imageBgr.Convert<Gray, byte>();
                imageSmoothBlur = imageGray.SmoothBlur(5, 5);
                imageBinary = imageSmoothBlur.ThresholdBinary(new Gray(150), new Gray(255));
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imageBinary, contours, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImage = imageBgr.Clone();
                for (int i = 0; i < contours.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contours[i]);
                    double area = rectangle.Width * rectangle.Height;
                    double aspectRatio = (double)rectangle.Width / rectangle.Height;
                    if (area > 50000 && area < 250000)
                        if (rectangle.Width > rectangle.Height && aspectRatio > 1 && aspectRatio < 2)
                            imagePlate = imageBgr.Copy(rectangle);
                }
                imagePlateGray = imagePlate.Convert<Gray, byte>();
                imagePlateSmoothBlur = imagePlateGray.SmoothBlur(3, 3);
                pictureBox_Screen.Image = imagePlateSmoothBlur.Bitmap;
            }
        }

        private void chinhSua_bienSoNhiPhan_Click(object sender, EventArgs e)
        {
            if (imageBgr == null)
            {
                MessageBox.Show("Bạn chưa có ảnh để chỉnh sửa!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                imageBgr.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
                imageGray = imageBgr.Convert<Gray, byte>();
                imageSmoothBlur = imageGray.SmoothBlur(5, 5);
                imageBinary = imageSmoothBlur.ThresholdBinary(new Gray(150), new Gray(255));
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imageBinary, contours, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImage = imageBgr.Clone();
                for (int i = 0; i < contours.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contours[i]);
                    double area = rectangle.Width * rectangle.Height;
                    double aspectRatio = (double)rectangle.Width / rectangle.Height;
                    if (area > 50000 && area < 250000)
                        if (rectangle.Width > rectangle.Height && aspectRatio > 1 && aspectRatio < 2)
                            imagePlate = imageBgr.Copy(rectangle);
                }
                imagePlateGray = imagePlate.Convert<Gray, byte>();
                imagePlateSmoothBlur = imagePlateGray.SmoothBlur(3, 3);
                imagePlateBinary = imagePlateSmoothBlur.ThresholdBinary(new Gray(120), new Gray(255));
                pictureBox_Screen.Image = imagePlateBinary.Bitmap;
            }
        }

        private void chinhSua_VeDuongVienKyTu_Click(object sender, EventArgs e)
        {
            if (imageBgr == null)
            {
                MessageBox.Show("Bạn chưa có ảnh để chỉnh sửa!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                imageBgr.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
                imageGray = imageBgr.Convert<Gray, byte>();
                imageSmoothBlur = imageGray.SmoothBlur(5, 5);
                imageBinary = imageSmoothBlur.ThresholdBinary(new Gray(150), new Gray(255));
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imageBinary, contours, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImage = imageBgr.Clone();
                for (int i = 0; i < contours.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contours[i]);
                    double area = rectangle.Width * rectangle.Height;
                    double aspectRatio = (double)rectangle.Width / rectangle.Height;
                    if (area > 50000 && area < 250000)
                        if (rectangle.Width > rectangle.Height && aspectRatio > 1 && aspectRatio < 2)
                            imagePlate = imageBgr.Copy(rectangle);
                }
                imagePlateGray = imagePlate.Convert<Gray, byte>();
                imagePlateSmoothBlur = imagePlateGray.SmoothBlur(3, 3);
                imagePlateBinary = imagePlateSmoothBlur.ThresholdBinaryInv(new Gray(140), new Gray(255));
                VectorOfVectorOfPoint contoursCharacter = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imagePlateBinary, contoursCharacter, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImagePlate = imagePlate.CopyBlank();
                for (int i = 0; i < contoursCharacter.Size; i++)                
                    CvInvoke.Polylines(drawImagePlate, contoursCharacter[i], true, new MCvScalar(255, 255, 255), 1);               
                pictureBox_Screen.Image = drawImagePlate.Bitmap;
            }
        }

        private void chinhSua_PhanVungKyTu_Click(object sender, EventArgs e)
        {
            if (imageBgr == null)
            {
                MessageBox.Show("Bạn chưa có ảnh để chỉnh sửa!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                imageBgr.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
                imageGray = imageBgr.Convert<Gray, byte>();
                imageSmoothBlur = imageGray.SmoothBlur(5, 5);
                imageBinary = imageSmoothBlur.ThresholdBinary(new Gray(150), new Gray(255));
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imageBinary, contours, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImage = imageBgr.Clone();
                for (int i = 0; i < contours.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contours[i]);
                    double area = rectangle.Width * rectangle.Height;
                    double aspectRatio = (double)rectangle.Width / rectangle.Height;
                    if (area > 50000 && area < 250000)
                        if (rectangle.Width > rectangle.Height && aspectRatio > 1 && aspectRatio < 2)
                            imagePlate = imageBgr.Copy(rectangle);
                }
                imagePlateGray = imagePlate.Convert<Gray, byte>();
                imagePlateSmoothBlur = imagePlateGray.SmoothBlur(3, 3);
                imagePlateBinary = imagePlateSmoothBlur.ThresholdBinaryInv(new Gray(140), new Gray(255));
                VectorOfVectorOfPoint contoursCharacter = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imagePlateBinary, contoursCharacter, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImagePlate = imagePlate.CopyBlank();
                for (int i = 0; i < contoursCharacter.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contoursCharacter[i]);
                    double area = rectangle.Width * rectangle.Height;
                    if (area < 2000 )
                        CvInvoke.Polylines(drawImagePlate, contoursCharacter[i], true, new MCvScalar(255, 0, 0), 1);
                    else
                        CvInvoke.Polylines(drawImagePlate, contoursCharacter[i], true, new MCvScalar(0, 255, 0), 1);
                }
                pictureBox_Screen.Image = drawImagePlate.Bitmap;
            }
        }

        private void chinhSua_LocVungPhamViKyTu_Click(object sender, EventArgs e)
        {
            if (imageBgr == null)
            {
                MessageBox.Show("Bạn chưa có ảnh để chỉnh sửa!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                imageBgr.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
                imageGray = imageBgr.Convert<Gray, byte>();
                imageSmoothBlur = imageGray.SmoothBlur(5, 5);
                imageBinary = imageSmoothBlur.ThresholdBinary(new Gray(150), new Gray(255));
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imageBinary, contours, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImage = imageBgr.Clone();
                for (int i = 0; i < contours.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contours[i]);
                    double area = rectangle.Width * rectangle.Height;
                    double aspectRatio = (double)rectangle.Width / rectangle.Height;
                    if (area > 50000 && area < 250000)
                        if (rectangle.Width > rectangle.Height && aspectRatio > 1 && aspectRatio < 2)
                            imagePlate = imageBgr.Copy(rectangle);
                }
                imagePlateGray = imagePlate.Convert<Gray, byte>();
                imagePlateSmoothBlur = imagePlateGray.SmoothBlur(3, 3);
                imagePlateBinary = imagePlateSmoothBlur.ThresholdBinaryInv(new Gray(140), new Gray(255));
                VectorOfVectorOfPoint contoursCharacter = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imagePlateBinary, contoursCharacter, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImagePlate = imagePlate.CopyBlank();
                for (int i = 0; i < contoursCharacter.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contoursCharacter[i]);
                    double area = rectangle.Width * rectangle.Height;
                    if (area < 2000)
                        CvInvoke.Polylines(drawImagePlate, contoursCharacter[i], true, new MCvScalar(255, 0, 0), 1);
                    else
                    {
                        CvInvoke.Polylines(drawImagePlate, contoursCharacter[i], true, new MCvScalar(0, 255, 0), 1);
                        CvInvoke.Rectangle(drawImagePlate, rectangle, new MCvScalar(0, 0, 255), 1);
                    }
                }
                pictureBox_Screen.Image = drawImagePlate.Bitmap;
            }
        }

        private void chinhSua_LocTiLeKyTu_Click(object sender, EventArgs e)
        {
            if (imageBgr == null)
            {
                MessageBox.Show("Bạn chưa có ảnh để chỉnh sửa!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                imageBgr.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
                imageGray = imageBgr.Convert<Gray, byte>();
                imageSmoothBlur = imageGray.SmoothBlur(5, 5);
                imageBinary = imageSmoothBlur.ThresholdBinary(new Gray(150), new Gray(255));
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imageBinary, contours, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImage = imageBgr.Clone();
                for (int i = 0; i < contours.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contours[i]);
                    double area = rectangle.Width * rectangle.Height;
                    double aspectRatio = (double)rectangle.Width / rectangle.Height;
                    if (area > 50000 && area < 250000)
                        if (rectangle.Width > rectangle.Height && aspectRatio > 1 && aspectRatio < 2)
                            imagePlate = imageBgr.Copy(rectangle);
                }
                imagePlateGray = imagePlate.Convert<Gray, byte>();
                imagePlateSmoothBlur = imagePlateGray.SmoothBlur(3, 3);
                imagePlateBinary = imagePlateSmoothBlur.ThresholdBinaryInv(new Gray(140), new Gray(255));
                VectorOfVectorOfPoint contoursCharacter = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imagePlateBinary, contoursCharacter, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImagePlate = imagePlate.CopyBlank();
                for (int i = 0; i < contoursCharacter.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contoursCharacter[i]);
                    double area = rectangle.Width * rectangle.Height;
                    if (area < 2000)
                        CvInvoke.Polylines(drawImagePlate, contoursCharacter[i], true, new MCvScalar(255, 0, 0), 1);
                    else
                    {
                        CvInvoke.Polylines(drawImagePlate, contoursCharacter[i], true, new MCvScalar(0, 255, 0), 1);
                        if (rectangle.Height > 30 && rectangle.Width > 10 && rectangle.Height < imagePlate.Height / 2 && rectangle.Width < rectangle.Height)
                            CvInvoke.Rectangle(drawImagePlate, rectangle, new MCvScalar(0, 0, 255), 1);
                    }
                }
                pictureBox_Screen.Image = drawImagePlate.Bitmap;
            }
        }

        private void chinhSua_GianKhoanCachKyTu_Click(object sender, EventArgs e)
        {
            if (imageBgr == null)
            {
                MessageBox.Show("Bạn chưa có ảnh để chỉnh sửa!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                imageBgr.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
                imageGray = imageBgr.Convert<Gray, byte>();
                imageSmoothBlur = imageGray.SmoothBlur(5, 5);
                imageBinary = imageSmoothBlur.ThresholdBinary(new Gray(150), new Gray(255));
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imageBinary, contours, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImage = imageBgr.Clone();
                for (int i = 0; i < contours.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contours[i]);
                    double area = rectangle.Width * rectangle.Height;
                    double aspectRatio = (double)rectangle.Width / rectangle.Height;
                    if (area > 50000 && area < 250000)
                        if (rectangle.Width > rectangle.Height && aspectRatio > 1 && aspectRatio < 2)
                            imagePlate = imageBgr.Copy(rectangle);
                }
                imagePlateGray = imagePlate.Convert<Gray, byte>();
                imagePlateSmoothBlur = imagePlateGray.SmoothBlur(3, 3);
                imagePlateBinary = imagePlateSmoothBlur.ThresholdBinaryInv(new Gray(140), new Gray(255));
                VectorOfVectorOfPoint contoursCharacter = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imagePlateBinary, contoursCharacter, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImagePlate = imagePlate.CopyBlank();
                int khoanCach = 5;
                for (int i = 0; i < contoursCharacter.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contoursCharacter[i]);
                    double area = rectangle.Width * rectangle.Height;
                    if (area < 2000)
                        CvInvoke.Polylines(drawImagePlate, contoursCharacter[i], true, new MCvScalar(255, 0, 0), 1);
                    else
                    {
                        CvInvoke.Polylines(drawImagePlate, contoursCharacter[i], true, new MCvScalar(0, 255, 0), 1);
                        
                        if (rectangle.Height > 30 && rectangle.Width > 10 && rectangle.Height < imagePlate.Height / 2 && rectangle.Width < rectangle.Height)
                            CvInvoke.Rectangle(drawImagePlate, rectangle, new MCvScalar(0, 0, 255), 1);
                        rectangle.X = rectangle.Location.X - khoanCach;
                        rectangle.Y = rectangle.Location.Y - khoanCach;
                        rectangle.Width = rectangle.Width + (2 * khoanCach);
                        rectangle.Height = rectangle.Height + (2 * khoanCach);
                        if (rectangle.Height > 30 && rectangle.Width > 10 && rectangle.Height < imagePlate.Height / 2 && rectangle.Width < rectangle.Height)
                            CvInvoke.Rectangle(drawImagePlate, rectangle, new MCvScalar(0, 255, 255), 1);
                    }

                }
                pictureBox_Screen.Image = drawImagePlate.Bitmap;
            }
        }

        private void chinhSua_VeVungKyTuBienSo_Click(object sender, EventArgs e)
        {
            if(imageBgr == null)
            {
                MessageBox.Show("Bạn chưa có ảnh để chỉnh sửa!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                imageBgr.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
                imageGray = imageBgr.Convert<Gray, byte>();
                imageSmoothBlur = imageGray.SmoothBlur(5, 5);
                imageBinary = imageSmoothBlur.ThresholdBinary(new Gray(150), new Gray(255));
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imageBinary, contours, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImage = imageBgr.Clone();
                for (int i = 0; i < contours.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contours[i]);
                    double area = rectangle.Width * rectangle.Height;
                    double aspectRatio = (double)rectangle.Width / rectangle.Height;
                    if (area > 50000 && area < 250000)
                        if (rectangle.Width > rectangle.Height && aspectRatio > 1 && aspectRatio < 2)
                            imagePlate = imageBgr.Copy(rectangle);
                }
                imagePlateGray = imagePlate.Convert<Gray, byte>();
                imagePlateSmoothBlur = imagePlateGray.SmoothBlur(3, 3);
                imagePlateBinary = imagePlateSmoothBlur.ThresholdBinaryInv(new Gray(140), new Gray(255));
                VectorOfVectorOfPoint contoursCharacter = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imagePlateBinary, contoursCharacter, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImagePlate = imagePlate.Clone();
                int khoanCach = 5;
                for (int i = 0; i < contoursCharacter.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contoursCharacter[i]);
                    double area = rectangle.Width * rectangle.Height;
                    if (area > 2000)
                    {
                        rectangle.X = rectangle.Location.X - khoanCach;
                        rectangle.Y = rectangle.Location.Y - khoanCach;
                        rectangle.Width = rectangle.Width + (2 * khoanCach);
                        rectangle.Height = rectangle.Height + (2 * khoanCach);
                        if (rectangle.Height > 30 && rectangle.Width > 10 && rectangle.Height < imagePlate.Height / 2 && rectangle.Width < rectangle.Height)
                            CvInvoke.Rectangle(drawImagePlate, rectangle, new MCvScalar(0, 255, 0), 1);
                    }    
                }
                pictureBox_Screen.Image = drawImagePlate.Bitmap;
            }
        }

        private void chinhSua_catVungKyTu_Click(object sender, EventArgs e)
        {
            if (imageBgr == null)
            {
                MessageBox.Show("Bạn chưa có ảnh để chỉnh sửa!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                imageBgr.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
                imageGray = imageBgr.Convert<Gray, byte>();
                imageSmoothBlur = imageGray.SmoothBlur(5, 5);
                imageBinary = imageSmoothBlur.ThresholdBinary(new Gray(150), new Gray(255));
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imageBinary, contours, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImage = imageBgr.Clone();
                for (int i = 0; i < contours.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contours[i]);
                    double area = rectangle.Width * rectangle.Height;
                    double aspectRatio = (double)rectangle.Width / rectangle.Height;
                    if (area > 50000 && area < 250000)
                        if (rectangle.Width > rectangle.Height && aspectRatio > 1 && aspectRatio < 2)
                            imagePlate = imageBgr.Copy(rectangle);
                }
                imagePlateGray = imagePlate.Convert<Gray, byte>();
                imagePlateSmoothBlur = imagePlateGray.SmoothBlur(3, 3);
                imagePlateBinary = imagePlateSmoothBlur.ThresholdBinaryInv(new Gray(140), new Gray(255));
                x_axisPosition = new List<int>();
                y_axisPosition = new List<int>();
                imageCharacter_Unsort = new List<Image<Bgr, byte>>();
                VectorOfVectorOfPoint contoursCharacter = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imagePlateBinary, contoursCharacter, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImagePlate = imagePlate.Clone();
                int khoanCach = 5;
                for (int i = 0; i < contoursCharacter.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contoursCharacter[i]);
                    double area = rectangle.Width * rectangle.Height;
                    if (area > 2000)
                    {
                        rectangle.X = rectangle.Location.X - khoanCach;
                        rectangle.Y = rectangle.Location.Y - khoanCach;
                        rectangle.Width = rectangle.Width + (2 * khoanCach);
                        rectangle.Height = rectangle.Height + (2 * khoanCach);
                        if (rectangle.Height > 30 && rectangle.Width > 10 && rectangle.Height < imagePlate.Height / 2 && rectangle.Width < rectangle.Height)
                        {
                            imageCharacter_Unsort.Add(imagePlate.Copy(rectangle));
                            x_axisPosition.Add(rectangle.Location.X);
                            y_axisPosition.Add(rectangle.Location.Y);
                        }    
                     }
                }
                for (int i = 0; i < imageCharacter_Unsort.Count; i++)                
                    CvInvoke.Imshow($"Ky tu {i + 1}", imageCharacter_Unsort[i]);               
                CvInvoke.WaitKey(0);
            }
        }
        public List<Image<Bgr, byte>> SortCharacters(Image<Bgr, byte> ImagePlate, List<Image<Bgr, byte>> imageCharacter_Unsort, List<int> x_axisPosition, List<int> y_axisPosition)
        {
            List<Image<Bgr, byte>> imageCharacter_Sort = new List<Image<Bgr, byte>>();
            List<(int index, int x)> Up_Left = new List<(int, int)>();
            List<(int index, int x)> Up_Right = new List<(int, int)>();
            List<(int index, int x)> Up_Right_Right = new List<(int, int)>();
            List<(int index, int x)> Down = new List<(int, int)>();

            //Tesseract 
            imageCityCode = new List<Image<Bgr, byte>>();
            imageLetter_1 = new List<Image<Bgr, byte>>();
            imageLetter_2 = new List<Image<Bgr, byte>>();
            imageNumberGroup = new List<Image<Bgr, byte>>();
        
            //************************************************************************************ 
        
            int plateWidth = ImagePlate.Width;
            int plateHeight = ImagePlate.Height;
            int midX = plateWidth / 2;
            int rightBoundary = midX + 80;
            int horizontalSplit = plateHeight / 3;
            for (int i = 0; i < imageCharacter_Unsort.Count; i++)            
                if (y_axisPosition[i] < horizontalSplit)           
                    if (x_axisPosition[i] < midX)
                        Up_Left.Add((i, x_axisPosition[i])); 
                    else if (x_axisPosition[i] < rightBoundary)
                        Up_Right.Add((i, x_axisPosition[i])); 
                    else
                        Up_Right_Right.Add((i, x_axisPosition[i]));               
                else                
                    Down.Add((i, x_axisPosition[i]));   
            Up_Left = Up_Left.OrderBy(item => item.x).ToList();
            Up_Right = Up_Right.OrderBy(item => item.x).ToList();
            Up_Right_Right = Up_Right_Right.OrderBy(item => item.x).ToList();
            Down = Down.OrderBy(item => item.x).ToList();
            foreach (var item in Up_Left)
            {
                imageCharacter_Sort.Add(imageCharacter_Unsort[item.index]);
                imageCityCode.Add(imageCharacter_Unsort[item.index]);
            }
            foreach (var item in Up_Right)
            {
                imageCharacter_Sort.Add(imageCharacter_Unsort[item.index]);
                imageLetter_1.Add(imageCharacter_Unsort[item.index]);
            }
            foreach (var item in Up_Right_Right)
            {
                imageCharacter_Sort.Add(imageCharacter_Unsort[item.index]);
                imageLetter_2.Add(imageCharacter_Unsort[item.index]);
            }
            foreach (var item in Down)
            {
                imageCharacter_Sort.Add(imageCharacter_Unsort[item.index]);
                imageNumberGroup.Add(imageCharacter_Unsort[item.index]);

            }
            return imageCharacter_Sort;
        }
        private void chinhSua_sapXepVungKyTu_Click(object sender, EventArgs e)
        {
            if (imageBgr == null)
            {
                MessageBox.Show("Bạn chưa có ảnh để chỉnh sửa!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                imageBgr.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
                imageGray = imageBgr.Convert<Gray, byte>();
                imageSmoothBlur = imageGray.SmoothBlur(5, 5);
                imageBinary = imageSmoothBlur.ThresholdBinary(new Gray(150), new Gray(255));
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imageBinary, contours, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImage = imageBgr.Clone();
                for (int i = 0; i < contours.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contours[i]);
                    double area = rectangle.Width * rectangle.Height;
                    double aspectRatio = (double)rectangle.Width / rectangle.Height;
                    if (area > 50000 && area < 250000)
                        if (rectangle.Width > rectangle.Height && aspectRatio > 1 && aspectRatio < 2)
                            imagePlate = imageBgr.Copy(rectangle);
                }
                imagePlateGray = imagePlate.Convert<Gray, byte>();
                imagePlateSmoothBlur = imagePlateGray.SmoothBlur(3, 3);
                imagePlateBinary = imagePlateSmoothBlur.ThresholdBinaryInv(new Gray(140), new Gray(255));
                x_axisPosition = new List<int>();
                y_axisPosition = new List<int>();
                imageCharacter_Unsort = new List<Image<Bgr, byte>>();
                VectorOfVectorOfPoint contoursCharacter = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imagePlateBinary, contoursCharacter, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImagePlate = imagePlate.Clone();
                int khoanCach = 5;
                for (int i = 0; i < contoursCharacter.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contoursCharacter[i]);
                    double area = rectangle.Width * rectangle.Height;
                    if (area > 2000)
                    {
                        rectangle.X = rectangle.Location.X - khoanCach;
                        rectangle.Y = rectangle.Location.Y - khoanCach;
                        rectangle.Width = rectangle.Width + (2 * khoanCach);
                        rectangle.Height = rectangle.Height + (2 * khoanCach);
                        if (rectangle.Height > 30 && rectangle.Width > 10 && rectangle.Height < imagePlate.Height / 2 && rectangle.Width < rectangle.Height)
                        {
                            imageCharacter_Unsort.Add(imagePlate.Copy(rectangle));
                            x_axisPosition.Add(rectangle.Location.X);
                            y_axisPosition.Add(rectangle.Location.Y);
                        }
                    }
                }
                imageCharacter_Sort = new List<Image<Bgr, byte>>();
                imageCharacter_Sort = SortCharacters(imagePlate, imageCharacter_Unsort, x_axisPosition, y_axisPosition);
                if (!Directory.Exists(saveFolder))
                {
                    Directory.CreateDirectory(saveFolder);
                }
                for (int i = 0; i < imageCharacter_Sort.Count; i++)
                {
                    CvInvoke.Imshow($"Ky tu {i + 1}", imageCharacter_Sort[i]);
                    string filePath = Path.Combine(saveFolder, $"{i + 1}.png");
                    imageCharacter_Sort[i].Save(filePath);
                }  
                    
                CvInvoke.WaitKey(0);
            }
        }

        private void chinhSua_NhanDangKyTuQuangHoc_Click(object sender, EventArgs e)
        {
            if (imageBgr == null)
            {
                MessageBox.Show("Bạn chưa có ảnh để chỉnh sửa!", "THÔNG BÁO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                imageBgr.Resize(640, 480, Emgu.CV.CvEnum.Inter.Linear);
                imageGray = imageBgr.Convert<Gray, byte>();
                imageSmoothBlur = imageGray.SmoothBlur(5, 5);
                imageBinary = imageSmoothBlur.ThresholdBinary(new Gray(150), new Gray(255));
                VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imageBinary, contours, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImage = imageBgr.Clone();
                for (int i = 0; i < contours.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contours[i]);
                    double area = rectangle.Width * rectangle.Height;
                    double aspectRatio = (double)rectangle.Width / rectangle.Height;
                    if (area > 50000 && area < 250000)
                        if (rectangle.Width > rectangle.Height && aspectRatio > 1 && aspectRatio < 2)
                            imagePlate = imageBgr.Copy(rectangle);
                }
                imagePlateGray = imagePlate.Convert<Gray, byte>();
                imagePlateSmoothBlur = imagePlateGray.SmoothBlur(3, 3);
                imagePlateBinary = imagePlateSmoothBlur.ThresholdBinaryInv(new Gray(140), new Gray(255));
                x_axisPosition = new List<int>();
                y_axisPosition = new List<int>();
                imageCharacter_Unsort = new List<Image<Bgr, byte>>();
                VectorOfVectorOfPoint contoursCharacter = new VectorOfVectorOfPoint();
                CvInvoke.FindContours(imagePlateBinary, contoursCharacter, null, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);
                Image<Bgr, byte> drawImagePlate = imagePlate.Clone();
                int khoanCach = 5;
                for (int i = 0; i < contoursCharacter.Size; i++)
                {
                    Rectangle rectangle = CvInvoke.BoundingRectangle(contoursCharacter[i]);
                    double area = rectangle.Width * rectangle.Height;
                    if (area > 2000)
                    {
                        rectangle.X = rectangle.Location.X - khoanCach;
                        rectangle.Y = rectangle.Location.Y - khoanCach;
                        rectangle.Width = rectangle.Width + (2 * khoanCach);
                        rectangle.Height = rectangle.Height + (2 * khoanCach);
                        if (rectangle.Height > 30 && rectangle.Width > 10 && rectangle.Height < imagePlate.Height / 2 && rectangle.Width < rectangle.Height)
                        {
                            imageCharacter_Unsort.Add(imagePlate.Copy(rectangle));
                            x_axisPosition.Add(rectangle.Location.X);
                            y_axisPosition.Add(rectangle.Location.Y);
                        }
                    }
                }
                imageCharacter_Sort = new List<Image<Bgr, byte>>();
                imageCharacter_Sort = SortCharacters(imagePlate, imageCharacter_Unsort, x_axisPosition, y_axisPosition);
                string strCityCode = string.Empty;
                string strLetter_1 = string.Empty;
                string strLetter_2 = string.Empty;  
                string strNumberGroup = string.Empty;

                if (imageCityCode != null) 
                { 
                    for (int i = 0; i < imageCityCode.Count; i++) {
                        Image<Gray, byte> imageCityCodeGray = imageCityCode[i].Convert<Gray,byte>();
                        Image<Gray, byte> imageCityCodeBinary = imageCityCodeGray.ThresholdBinary(new Gray(140), new Gray(255));

                        strCityCode += TesseractNumbers(imageCityCodeBinary);
                    }
                }

                if (imageLetter_1 != null)
                {
                    for (int i = 0; i < imageLetter_1.Count; i++)
                    {
                        Image<Gray, byte> imageLetter_1Gray = imageLetter_1[i].Convert<Gray, byte>();
                        Image<Gray, byte> imageLetter_1Binary = imageLetter_1Gray.ThresholdBinary(new Gray(140), new Gray(255));
                        strLetter_1 += TesseractCharacters(imageLetter_1Binary);
                    }
                }
                if (imageLetter_2 != null)
                {  
                    for (int i = 0; i < imageLetter_2.Count; i++)
                    {
                        Image<Gray, byte> imageLetter_2Gray = imageLetter_2[i].Convert<Gray, byte>();
                        Image<Gray, byte> imageLetter_2Binary = imageLetter_2Gray.ThresholdBinary(new Gray(140), new Gray(255));
                        strLetter_2 += TesseractNumbers_Characters(imageLetter_2Binary);
                    }
                }
                if (imageNumberGroup != null)
                {
                    for (int i = 0; i < imageNumberGroup.Count; i++)
                    {
                        Image<Gray, byte> imageNumberGroupGray = imageNumberGroup[i].Convert<Gray, byte>();
                        Image<Gray, byte> imageNumberGroupBinary = imageNumberGroupGray.ThresholdBinary(new Gray(140), new Gray(255));

                        strNumberGroup += TesseractNumbers(imageNumberGroupBinary);
                    }
                }
                MessageBox.Show("Biển số xe: " + strCityCode + "-" + strLetter_1 + strLetter_2 + strNumberGroup, "KẾT QUẢ NHẬN DIỆN");
            }
        }


        private string TesseractCharacters(Image<Gray, byte> image)
        {
            if (image == null)
                return "";
            using (var engine = new TesseractEngine(tessDataPath, "huy", EngineMode.Default))
            {
                engine.SetVariable("tessedit_char_whitelist", "ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                engine.SetVariable("tessedit_pageseg_mode", "7");

                using (var img = PixConverter.ToPix(image.Bitmap))
                {
                    using (var page = engine.Process(img, PageSegMode.SingleLine))
                    {
                        string text = page.GetText().Trim();
                        return string.IsNullOrWhiteSpace(text) ? "?" : text;
                    }
                }
            }
        }


        private string TesseractNumbers_Characters(Image<Gray, byte> image)
        {
            if (image == null)
                return "";
            using (var engine = new TesseractEngine(tessDataPath, "huy", EngineMode.Default))
            {
                engine.SetVariable("tessedit_char_whitelist", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                engine.SetVariable("tessedit_pageseg_mode", "7");

                using (var img = PixConverter.ToPix(image.Bitmap))
                {
                    using (var page = engine.Process(img, PageSegMode.SingleLine))
                    {
                        string text = page.GetText().Trim();
                        return string.IsNullOrWhiteSpace(text) ? "?" : text;
                    }
                }
            }
        }
        private string TesseractNumbers(Image<Gray, byte> image)
        {
            if (image == null)
                return "";
            using (var engine = new TesseractEngine(tessDataPath, "huy", EngineMode.Default))
            {
                engine.SetVariable("tessedit_char_whitelist", "0123456789");
                engine.SetVariable("tessedit_pageseg_mode", "7");

                using (var img = PixConverter.ToPix(image.Bitmap))
                {
                    using (var page = engine.Process(img, PageSegMode.SingleLine))
                    {
                        string text = page.GetText().Trim();
                        return string.IsNullOrWhiteSpace(text) ? "?" : text;
                    }
                }
            }
        }

      

    }
}
