using CarSalesSystem.Admin.Pages;
using CarSalesSystem.Admin.User_Controls;
using CarSalesSystem.Admin.Windows;
using CarSalesSystem.Model;
using CarSalesSystem.Viewmodel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Globalization;
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
        private int checkNewAccount = 0;
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
        public ICommand  CaculateTotalCommand { get; set; }
        public ICommand ImportProductSaveCommand { get; set; }
        public ICommand BuyProductCommand { get; set; }
        public ICommand SearchCustomerInfoCommand { get; set; }
        public ICommand AddCustomerInfoCommand { get; set; }
        public ICommand CaculatePriceProductCommand { get; set; }
        public ProductViewModel()
        {
            EditProductCommand = new RelayCommand<ProductControl>((parameter) => true, (parameter) => ClickEditProduct(parameter));
            BuyProductCommand = new RelayCommand<ProductControl>((parameter) => true, (parameter) => ClickBuyProduct(parameter));

            ShowLoadProductCommand = new RelayCommand<ProductPG>((parameter) => true, (parameter) => ShowLoadProduct(parameter));
            ClickAddCommand = new RelayCommand<ProductPG>((parameter) => true, (parameter) => ShowAddProduct(parameter));

            BackAddProductCommand = new RelayCommand<Addproduct>((parameter) => true, (parameter) => parameter.Close());
            AddProductCommand = new RelayCommand<Addproduct>((parameter) => true, (parameter) => AddProduct(parameter));

            BackEditProductCommand = new RelayCommand<EditProduct>((parameter) => true, (parameter) => parameter.Close());
            UpdateProductCommand = new RelayCommand<EditProduct>((parameter) => true, (parameter) => UpdateProduct(parameter));

            UpLoadIMGEditCommand = new RelayCommand<Grid>((parameter) => true, (parameter) => UpLoadIMGEdit(parameter));

            BackImportProductCommand = new RelayCommand<ImportProduct>((parameter) => true, (parameter) => parameter.Close());

            ShowLoadImportProductCommand = new RelayCommand<WarehousePG>((parameter) => true, (parameter) => ShowLoadImportProduct(parameter));
            ClickImportProductCommand = new RelayCommand<ImportControl>((parameter) => true, (parameter) => ShowImportProduct(parameter));
            CaculateTotalCommand = new RelayCommand<ImportProduct>((parameter) => true, (parameter) => CaculateTotal(parameter));
            ImportProductSaveCommand = new RelayCommand<ImportProduct>((parameter) => true, (parameter) => ImportProductSave(parameter));

            SearchCustomerInfoCommand = new RelayCommand<BuyProductEmp>((parameter) => true, (parameter) => SearchCustomerInfo(parameter));
            AddCustomerInfoCommand = new RelayCommand<BuyProductEmp>((parameter) => true, (parameter) => AddCustomerInfo(parameter));
            CaculatePriceProductCommand = new RelayCommand<BuyProductEmp>((parameter) => true, (parameter) => CaculatePriceProduct(parameter));
        }

        private void CaculatePriceProduct(BuyProductEmp parameter)
        {
            decimal price;
            decimal.TryParse(parameter.tbPriceProduct.Text, NumberStyles.Currency, CultureInfo.CurrentCulture.NumberFormat, out price);
            int percent = int.Parse(parameter.tbDiscount.Text);
            Console.WriteLine(percent);
            if (percent != 0)
            {
                decimal discount = (decimal)(price * percent) / 100;
                decimal tong = Math.Truncate(price - discount);
                parameter.tbTotalBill.Text = (tong).ToString("C", CultureInfo.CurrentCulture);
                Console.WriteLine(tong);
            }
            else parameter.tbTotalBill.Text = (price).ToString("C", CultureInfo.CurrentCulture);

        }


        private void AddCustomerInfo(BuyProductEmp parameter)
        {
            if (checkNewAccount == 0)
            {
                ACCOUNT account = new ACCOUNT();
                account.USERNAME = parameter.tbPhone.Text;
                account.PASS = "123";
                account.TYPE_USER = 2;
                try
                {
                    DataProvider.Ins.DB.ACCOUNTs.Add(account);
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
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["CarSalesSystem.Properties.Settings.CARSALESSYSTEMConnectionString"].ConnectionString;
                try
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"INSERT INTO CUSTOMER(CUS_ACCOUNT,CUS_NAME,PHONE,CUS_ADDRESS,RANK_ID,REGIST_DATE,REVENUE,PRODUCT_NUMBER) 
                        VALUES(@CUS_ACCOUNT,@CUS_NAME,@PHONE,@CUS_ADDRESS,@RANK_ID,@REGIST_DATE,@REVENUE,@PRODUCT_NUMBER)";
                        command.Parameters.AddWithValue("@CUS_ACCOUNT", parameter.tbPhone.Text);
                        command.Parameters.AddWithValue("@CUS_NAME", parameter.tbName.Text);
                        command.Parameters.AddWithValue("@PHONE", parameter.tbPhone.Text);
                        command.Parameters.AddWithValue("@CUS_ADDRESS", parameter.tbAddress.Text);
                        command.Parameters.AddWithValue("@RANK_ID", "R00");
                        command.Parameters.AddWithValue("@REGIST_DATE",DateTime.UtcNow.ToString());
                        command.Parameters.AddWithValue("@REVENUE", 0);
                        command.Parameters.AddWithValue("@PRODUCT_NUMBER", 0);
                        command.ExecuteNonQuery();
                    }
                }
                finally
                {
                    connection.Close();
                }
              
            }
        }

        private void SearchCustomerInfo(BuyProductEmp parameter)
        {
            var cusInfo = DataProvider.Ins.DB.CUSTOMERs.Where(x => x.PHONE == parameter.tbPhone.Text ).FirstOrDefault();
            if (cusInfo != null)
            {
                parameter.tbName.Text = cusInfo.CUS_NAME;
                parameter.tbAddress.Text = cusInfo.CUS_ADDRESS;
                parameter.tbDiscount.Text = cusInfo.RANK_MONEY.DISCOUNT.ToString();
                checkNewAccount = 1;
            }
            else
            {
                checkNewAccount = 0;
                parameter.tbDiscount.Text = "0";
            }
        }

        private void ClickBuyProduct(ProductControl parameter)
        {
            this.productControl = parameter;
            idProduct = parameter.tbNo.Text;
            ShowBuyProduct(idProduct);
        }

        private void ShowBuyProduct(string idProduct)
        {
            BuyProductEmp buyProduct = new BuyProductEmp();
            buyProduct.Title = "Buy Product";
            var productInfo = DataProvider.Ins.DB.PRODUCTs.Find(idProduct);
            buyProduct.pdSell.Text = DateTime.UtcNow.Date.ToString();
            var product = DataProvider.Ins.DB.PRODUCTs.Find(idProduct);
            buyProduct.tbNameProduct.Text = product.PRO_NAME;
            buyProduct.tbAmountProduct.Text = "1";
            buyProduct.tbPriceProduct.Text = product.PRICE.ToString("C", CultureInfo.CurrentCulture);
            buyProduct.ShowDialog();
        }

        private void ImportProductSave(ImportProduct parameter)
        {
            IMPORTRECEIPT importreceipt = new IMPORTRECEIPT();
            IMPORTRECEIPTINFO importreceiptinfo = new IMPORTRECEIPTINFO();
            var product = DataProvider.Ins.DB.PRODUCTs.Find(parameter.tbIdProduct.Text);
            product.STORAGE_NUMBER += int.Parse(parameter.tbQuantity.Text);
            importreceipt.IRECEIPT_ID = parameter.tbIdReceipt.Text;
            importreceipt.EMPLOYEE_ID = parameter.tbIdEmployee.Text;
            importreceipt.DATETIME_IMPORT = DateTime.Parse(parameter.pdDateTime.Text);
            decimal value;
            decimal.TryParse(parameter.tbTotalPrice.Text, NumberStyles.Currency, CultureInfo.CurrentCulture.NumberFormat, out value);
            importreceipt.TOTAL_PRICE = value;

            importreceiptinfo.IRECEIPT_ID = parameter.tbIdReceipt.Text;
            importreceiptinfo.PRO_ID=parameter.tbIdProduct.Text;
            importreceiptinfo.QUANTITY = int.Parse(parameter.tbQuantity.Text);
            importreceiptinfo.IMPORT_PRICE = decimal.Parse(parameter.tbImportPrice.Text);
            try
            {
                DataProvider.Ins.DB.IMPORTRECEIPTs.Add(importreceipt);
                DataProvider.Ins.DB.IMPORTRECEIPTINFOes.Add(importreceiptinfo);
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
            ShowLoadImportProduct(warehousePG);
            parameter.Close();
        }

        private void CaculateTotal(ImportProduct parameter)
        {
            long price=0;
            int quantity = 0;
            int.TryParse(parameter.tbImportPrice.Text, out quantity);
            long.TryParse(parameter.tbQuantity.Text, out price);
            parameter.tbTotalPrice.Text = (price*quantity).ToString("C", CultureInfo.CurrentCulture);
            
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
            var listReceipt = DataProvider.Ins.DB.IMPORTRECEIPTs.Count() + 1;
            ImportProduct importProduct = new ImportProduct();
            importProduct.Title = "Import Product";
            importProduct.tbName.Text = productInfo.PRO_NAME;
            importProduct.tbYear.Text = productInfo.PRO_YEAR.ToString();
            importProduct.pdDateTime.Text = DateTime.UtcNow.Date.ToString();
            if (listReceipt < 10)
            {
                importProduct.tbIdReceipt.Text = "NH0" + listReceipt.ToString();
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
            importProduct.tbIdEmployee.Text = AccountInfo.IdAccount;
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
            var type = DataProvider.Ins.DB.TYPEPRODUCTs.Where(x => x.TYPEPRODUCT_NAME == parameter.cbType.Text).FirstOrDefault();
            var producer = DataProvider.Ins.DB.PRODUCERs.Where(x => x.PRODUCER_NAME == parameter.tbProducer.Text).FirstOrDefault();
            numberproduct++;
            string id = "";
            if (numberproduct < 10)
            {
                id += "P00" + numberproduct.ToString();
            }
            else if (numberproduct < 100)
            {
                id += "P0" + numberproduct.ToString();
            }
            else if (numberproduct < 1000)
            {
                id += "P" + numberproduct.ToString();
            }
            PRODUCT ProductInfo = new PRODUCT();
            ProductInfo.PRO_ID = id;
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
            ProductInfo.DISPLACEMENT = int.Parse(parameter.tbDispla.Text);
            ProductInfo.MAXSPEED = int.Parse(parameter.tbMaxSpeed.Text);
            ProductInfo.ACCELERATION = parameter.tbAcce.Text;
            ProductInfo.TRACTION = parameter.tbTraction.Text;
            ProductInfo.PRICE = decimal.Parse(parameter.tbPrice.Text);
            ProductInfo.PRODUCER.PRODUCER_NAME = parameter.tbProducer.Text;
            ProductInfo.ENGINELAYOUT = parameter.tbEngine.Text;
            ProductInfo.PRO_NAME = parameter.tbName.Text;
            ProductInfo.PRO_YEAR = int.Parse(parameter.cbYear.Text);
            if (imagefilename != null)
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
            foreach (var product in listProduct)
            {
                ProductControl productControl = new ProductControl();
                flat = !flat;
                if (flat)
                {
                    productControl.grMain.Background = (Brush)new BrushConverter().ConvertFrom("#FFFFFF");
                }
                productControl.tbName.Text = product.PRO_NAME;
                productControl.tbNo.Text = product.PRO_ID;
                productControl.tbType.Text = product.TYPEPRODUCT.TYPEPRODUCT_NAME.ToString();
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
            editProduct.tbName.Text = productInfo.PRO_NAME;
            editProduct.tbProducer.Text = productInfo.PRODUCER.PRODUCER_NAME;
            editProduct.tbEngine.Text = productInfo.ENGINELAYOUT;
            editProduct.tbDispla.Text = productInfo.DISPLACEMENT.ToString();
            editProduct.tbMaxSpeed.Text = productInfo.MAXSPEED.ToString();
            editProduct.tbMaxPower.Text = productInfo.MAXPOWER.ToString();
            editProduct.tbAcce.Text = productInfo.ACCELERATION;
            editProduct.tbTraction.Text = productInfo.TRACTION;
            editProduct.tbPrice.Text = ((int)productInfo.PRICE).ToString();
            if (productInfo.IMG != null)
            {
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = Converter.Instance.ToImage(productInfo.IMG);
                editProduct.grdSelectImage.Background = imageBrush;
            }
            editProduct.ShowDialog();
        }
    }
}