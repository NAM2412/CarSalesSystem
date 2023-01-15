using CarSalesSystem.Admin.Pages;
using CarSalesSystem.Admin.User_Controls;
using CarSalesSystem.Admin.Windows;
using CarSalesSystem.Model;
using CarSalesSystem.Viewmodel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CarSalesSystem.ViewModel
{
    public class ProductViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private ProductControl productControl;
        private ImportControl importControl;
        private int numberproduct;
        private ProductPG productPG;
        private WarehousePG warehousePG;
        private Addproduct Addproduct;
        private String imagefilename;
        private String idProduct;
        public ICommand EditProductCommand { get; set; }
        public ICommand ShowLoadProductCommand { get; set; }
        public ICommand ClickAddCommand { get; set; }
        public ICommand BackAddProductCommand { get; set; }
        public ICommand BackEditProductCommand { get; set; }
        public ICommand UpdateProductCommand { get; set; }
        public ICommand UpLoadIMGEditCommand { get; set; }
        public ICommand AddProductCommand { get; set; }
        public ICommand ShowLoadImportProductCommand { get; set; }
        public ICommand ClickImportProductCommand { get; set; }
        public ICommand BackImportProductCommand { get; set; }
        public ProductViewModel()
        {
            EditProductCommand = new RelayCommand<ProductControl>((parameter) => true, (parameter) => ClickEditProduct(parameter));

            ShowLoadProductCommand = new RelayCommand<ProductPG>((parameter) => true, (parameter) => ShowLoadProduct(parameter));
            ClickAddCommand = new RelayCommand<ProductPG>((parameter) => true, (parameter) => ShowAddProduct(parameter));

            BackAddProductCommand = new RelayCommand<Addproduct>((parameter) => true, (parameter) => parameter.Close());
            AddProductCommand = new RelayCommand<Addproduct>((parameter) => true, (parameter) => AddProduct(parameter));

            BackEditProductCommand = new RelayCommand<EditProduct>((parameter) => true, (parameter) => parameter.Close());
            UpdateProductCommand = new RelayCommand<EditProduct>((parameter) => true, (parameter) => UpdateProduct(parameter));

            UpLoadIMGEditCommand = new RelayCommand<Grid>((parameter) => true, (parameter) => UpLoadIMGEdit(parameter));

            BackImportProductCommand = new RelayCommand<ImportProduct>((parameter) => true, (parameter) => parameter.Close()) ;

            ShowLoadImportProductCommand = new RelayCommand<WarehousePG>((parameter) => true, (parameter) => ShowLoadImportProduct(parameter));
            ClickImportProductCommand = new RelayCommand<ImportControl>((parameter) => true, (parameter) => ShowImportProduct(parameter));
        }

        private void ShowImportProduct(ImportControl parameter)
        {
            this.importControl = parameter;
            idProduct = parameter.tbNo.Text;
            showImportProductWindow(idProduct);
        }

        private void showImportProductWindow(string idProduct)
        {
            var productInfo = DataProvider.Ins.DB.PRODUCTs.Find(idProduct);
            var listReceipt = DataProvider.Ins.DB.IMPORTRECEIPTs.Count()+1;
            ImportProduct importProduct = new ImportProduct();
            importProduct.Title = "Import Product";
            importProduct.tbName.Text = productInfo.PRO_NAME;
            importProduct.tbYear.Text = productInfo.PRO_YEAR.ToString();
            importProduct.pdDateTime.Text = DateTime.UtcNow.Date.ToString();
            if(listReceipt < 10)
            {
                importProduct.tbIdReceipt.Text = "NH0"+ listReceipt.ToString();
            }
            else
            {
                importProduct.tbIdReceipt.Text = "NH" + listReceipt.ToString();
            }
            importProduct.tbIdProduct.Text = idProduct;
            if (productInfo.IMG != null)
            {
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = Converter.Instance.ToImage(productInfo.IMG);
                importProduct.grdSelectImage.Background = imageBrush;
            }
            importProduct.ShowDialog();
            
        }

        private void ShowLoadImportProduct(WarehousePG parameter)
        {
            this.warehousePG = parameter;
            parameter.skpProduct.Children.Clear();
            var listProduct = DataProvider.Ins.DB.PRODUCTs.ToList();
            bool flat = false;
            int i = 0;
            foreach (var product in listProduct)
            {
                ImportControl productControl = new ImportControl();
                flat = !flat;
                if (flat)
                {
                    productControl.grMain.Background = (Brush)new BrushConverter().ConvertFrom("#FFFFFF");
                }
                productControl.tbName.Text = product.PRO_NAME;
                productControl.tbNo.Text = product.PRO_ID;
                productControl.tbYear.Text = product.PRO_YEAR.ToString();
                productControl.tbAmount.Text = (product.STORAGE_NUMBER - product.SELL_NUMBER).ToString();
                parameter.skpProduct.Children.Add(productControl);
                i++;
            }
            numberproduct = i;

        }

        private void AddProduct(Addproduct parameter)
        {
            var type = DataProvider.Ins.DB.TYPEPRODUCTs.Where(x => x.TYPEPRODUCT_NAME== parameter.cbType.Text).FirstOrDefault();
            var producer  = DataProvider.Ins.DB.PRODUCERs.Where(x => x.PRODUCER_NAME == parameter.tbProducer.Text).FirstOrDefault();
            numberproduct++;
            string id="";
            if(numberproduct < 10)
            {
                id += "P00" + numberproduct.ToString();
            }
            else if(numberproduct < 100)
            {
                id += "P0" + numberproduct.ToString();
            }
            else if (numberproduct < 1000)
            {
                id += "P" + numberproduct.ToString();
            }
            PRODUCT ProductInfo = new PRODUCT();
            ProductInfo.PRO_ID= id;
            ProductInfo.MAXPOWER = int.Parse(parameter.tbMaxPower.Text);
            ProductInfo.DISPLACEMENT = int.Parse(parameter.tbDispla.Text);
            ProductInfo.MAXSPEED = int.Parse(parameter.tbMaxSpeed.Text);
            ProductInfo.ACCELERATION = parameter.tbAcce.Text;
            ProductInfo.TRACTION = parameter.tbTraction.Text;
            ProductInfo.PRICE = decimal.Parse(parameter.tbPrice.Text);
            ProductInfo.PRODUCER_ID = producer.PRODUCER_ID;
            ProductInfo.TYPEPRO_ID = type.TYPEPRODUCT_ID;
            ProductInfo.ENGINELAYOUT = parameter.tbEngine.Text;
            ProductInfo.PRO_NAME = parameter.tbName.Text;
            ProductInfo.STORAGE_NUMBER = 0;
            ProductInfo.SELL_NUMBER = 0;
            ProductInfo.PRO_YEAR = int.Parse(parameter.cbYear.Text);
            if (imagefilename != null)
                ProductInfo.IMG = Converter.Instance.StreamFile(imagefilename);
           
            try
            {
                DataProvider.Ins.DB.PRODUCTs.Add(ProductInfo);
                DataProvider.Ins.DB.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            ShowLoadProduct(productPG);
            parameter.Close();
        }

        private void UpLoadIMGEdit(Grid parameter)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == DialogResult.OK == true)
            {
                imagefilename = op.FileName;
                ImageBrush imageBrush = new ImageBrush();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(imagefilename);
                bitmap.EndInit();
                imageBrush.ImageSource = bitmap;
                parameter.Background = imageBrush;
            }
        }

        private void UpdateProduct(EditProduct parameter)
        {
            var ProductInfo = DataProvider.Ins.DB.PRODUCTs.Find(idProduct);
            ProductInfo.MAXPOWER = int.Parse(parameter.tbMaxPower.Text);
            ProductInfo.DISPLACEMENT =int.Parse( parameter.tbDispla.Text);
            ProductInfo.MAXSPEED= int.Parse(parameter.tbMaxSpeed.Text);
            ProductInfo.ACCELERATION= parameter.tbAcce.Text;
            ProductInfo.TRACTION = parameter.tbTraction.Text;
            ProductInfo.PRICE = decimal.Parse(parameter.tbPrice.Text);
            ProductInfo.PRODUCER.PRODUCER_NAME = parameter.tbProducer.Text;
            ProductInfo.ENGINELAYOUT = parameter.tbEngine.Text;
            ProductInfo.PRO_NAME = parameter.tbName.Text;
            ProductInfo.PRO_YEAR = int.Parse(parameter.cbYear.Text);
            if(imagefilename != null)
            ProductInfo.IMG = Converter.Instance.StreamFile(imagefilename);
            DataProvider.Ins.DB.SaveChanges();
            ShowLoadProduct(productPG);
            parameter.Close();


        }

        private void ShowAddProduct(ProductPG parameter)
        {
            Addproduct addproduct = new Addproduct();
            addproduct.Title = "Add Product";
            int year = DateTime.Now.Year;
            for (int i = 1800; i <= year; i++)
                addproduct.cbYear.Items.Add(i);
            var listProducer = DataProvider.Ins.DB.PRODUCERs.ToList();
            var listtype = DataProvider.Ins.DB.TYPEPRODUCTs.ToList();
            foreach (var item in listProducer)
                addproduct.tbProducer.Items.Add(item.PRODUCER_NAME);
            foreach (var item in listtype)
                addproduct.cbType.Items.Add(item.TYPEPRODUCT_NAME);
            addproduct.ShowDialog();

        }

        private void ShowLoadProduct(ProductPG parameter)
        {
           this.productPG = parameter;
            parameter.skpProduct.Children.Clear();
            var listProduct = DataProvider.Ins.DB.PRODUCTs.ToList();
            bool flat = false;
            int i = 0;
            foreach(var product in listProduct)
            {
                ProductControl productControl = new ProductControl();
                flat = !flat;
                if (flat)
                {
                    productControl.grMain.Background = (Brush)new BrushConverter().ConvertFrom("#FFFFFF");
                }
                productControl.tbName.Text = product.PRO_NAME;
                productControl.tbNo.Text = product.PRO_ID;
                productControl.tbType.Text = product.TYPEPRODUCT.TYPEPRODUCT_NAME;
                productControl.tbYear.Text = product.PRO_YEAR.ToString();
                parameter.skpProduct.Children.Add(productControl);
                i++;
            }
            numberproduct = i;
        }

        private void ClickEditProduct(ProductControl parameter)
        {
            this.productControl = parameter;
            idProduct = parameter.tbNo.Text;
            ShowEditProduct(idProduct);
        }

        private void ShowEditProduct(string idProduct)
        {
            EditProduct editProduct = new EditProduct();
            editProduct.Title = "Edit the information of Car";
            var productInfo = DataProvider.Ins.DB.PRODUCTs.Find(idProduct);
            int year = DateTime.Now.Year;
            for (int i = 1800; i <= year; i++)
                editProduct.cbYear.Items.Add(i);
            editProduct.cbYear.Text = productInfo.PRO_YEAR.ToString();
            editProduct.tbName.Text= productInfo.PRO_NAME;
            editProduct.tbProducer.Text= productInfo.PRODUCER.PRODUCER_NAME;
            editProduct.tbEngine.Text= productInfo.ENGINELAYOUT;
            editProduct.tbDispla.Text=productInfo.DISPLACEMENT.ToString();
            editProduct.tbMaxSpeed.Text=productInfo.MAXSPEED.ToString();
            editProduct.tbMaxPower.Text= productInfo.MAXPOWER.ToString();
            editProduct.tbAcce.Text=productInfo.ACCELERATION;
            editProduct.tbTraction.Text=productInfo.TRACTION;
            editProduct.tbPrice.Text=((int)productInfo.PRICE).ToString();
            if(productInfo.IMG != null)
            {
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource= Converter.Instance.ToImage(productInfo.IMG);
                editProduct.grdSelectImage.Background = imageBrush;
            }
            editProduct.ShowDialog();
        }
    }
}
