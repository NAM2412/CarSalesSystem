using CarSalesSystem.Admin.Pages;
using CarSalesSystem.Customer.Windows;
using CarSalesSystem.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarSalesSystem.Customer.Pages
{
    /// <summary>
    /// Interaction logic for Store.xaml
    /// </summary>
    public partial class Store : Page
    {
        ObservableCollection<PRODUCT> products;
        CUSTOMER customer;
    
        public Store(CUSTOMER cus)
        {
            InitializeComponent();
            customer = cus;
            Thread thread = new Thread(delegate ()
            {
                // Get và xác định số trang
                var db = new CARSALESSYSTEMEntities();
                int numPages = (int)Math.Ceiling(db.PRODUCTs.Count() / 10.0);
                if (numPages == 0) numPages = 1;

                // Đưa vào Combobox
                List<string> pageNumber = new List<string>();
                for (int i = 1; i <= numPages; i++)
                {
                    pageNumber.Add(i + "/" + numPages);
                }
                Dispatcher.Invoke(() =>
                {
                    cbPage.ItemsSource = pageNumber;
                    cbPage.SelectedIndex = 0;
                });

                // Lấy danh sách sản phẩm
                products = new ObservableCollection<PRODUCT>(db.PRODUCTs);
                for (int i = 0; i < products.Count; i++)
                {
                    for (int j = i; j < products.Count; j++)
                    {
                        if (compare_sort(products[i], products[j]))
                        {
                            PRODUCT temp = products[i];
                            products[i] = products[j];
                            products[j] = temp;
                        }
                    }
                }
                // Cập nhật UI
                Dispatcher.Invoke(() =>
                {
                    listProduct.ItemsSource = products.Skip(cbPage.SelectedIndex * 10).Take(10);
                });
            });
            thread.Start();
        }

        private bool compare_sort(PRODUCT pRODUCT1, PRODUCT pRODUCT2)
        {
            int sortType = 0;
            Dispatcher.Invoke(() => { sortType = cbSort.SelectedIndex; });
            switch (sortType)
            {
                case 2: return pRODUCT1.PRICE > pRODUCT2.PRICE; // Tăng dần giá cả
                case 3: return pRODUCT1.PRICE < pRODUCT2.PRICE; // Giảm dần giá cả
                default: return true;
            }
        }

        private void ListProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PRODUCT prd = listProduct.SelectedItem as PRODUCT;
            DetailCar detailCar = new DetailCar(prd, customer);
            detailCar.Show();
        }


        private void txbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Tạo mới danh sách sản phẩm có tên chứa nội dung ô tìm kiếm
            ObservableCollection<PRODUCT> searchProducts = new ObservableCollection<PRODUCT>();
            // Nếu ô tìm kiếm rỗng, thì lấy tất cả sản phẩm
            if (txbSearch.Text.Length == 0)
            {
                refresh(true);
            }

            // Nếu ô tìm kiếm có nội dung
            else
            {
                for (int i = 0; i < products.Count; i++)
                {
                    if (products[i].PRO_NAME.ToUpper().Contains(txbSearch.Text.ToUpper())) // Nếu tìm thấy tên phù hợp
                    {
                        searchProducts.Add(products[i]); // Thì thêm vào danh sách mới
                    }
                }

                // Nếu tìm thấy ít nhất 1 sản phẩm thì hiển thị, không thì thông báo
                if (searchProducts.Count > 0)
                {
                    reset_page(searchProducts);
                    listProduct.Visibility = Visibility.Visible;
                    listProduct.ItemsSource = searchProducts;

                }
                else
                {
                    listProduct.Visibility = Visibility.Hidden;
                }
            }
        }

        #region reset and refresh list
        private void reset_page(ObservableCollection<PRODUCT> searchProducts)
        {
            List<string> pageNumber = new List<string>();
            int numPages = (int)Math.Ceiling(products.Count() / 10.0);
            if (numPages == 0) numPages = 1;

            for (int i = 1; i <= numPages; i++)
            {
                pageNumber.Add(i + "/" + numPages);
            }
            Dispatcher.Invoke(() =>
            {
                cbPage.ItemsSource = pageNumber;
                cbPage.SelectedIndex = 0;
            });
        }

        public void refresh(bool Data)
        {
            if (!Data) return;


            // Nếu lượng sản phẩm thêm vào nhiều và tạo thành trang mới
            int curPage = cbPage.SelectedIndex;
            int newNumPages = (int)Math.Ceiling(products.Count / 10.0);
            List<string> pageNumber = new List<string>();
            for (int j = 1; j <= newNumPages; j++)
            {
                pageNumber.Add(j + "/" + newNumPages);
            }
            cbPage.ItemsSource = pageNumber;
            cbPage.SelectedIndex = curPage;
            listProduct.ItemsSource = products.Skip(cbPage.SelectedIndex * 10).Take(10);
        }
        #endregion

        #region combobox producer effect
        private void cbProducer_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.Background = Brushes.Transparent;
        }

        private void cbProducer_DropDownOpened(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.Background = Brushes.LightGray;
        }
        #endregion

        #region Turn page
        private void btLeft_Click(object sender, RoutedEventArgs e)
        {
            if (products != null)
            {
                if (cbPage.SelectedIndex > 0)
                {
                    listProduct.ItemsSource = products.Skip(--cbPage.SelectedIndex * 10).Take(10);
                }
            }
        }

        private void btRight_Click(object sender, RoutedEventArgs e)
        {
            if (products != null)
            {
                if (cbPage.SelectedIndex < cbPage.Items.Count - 1)
                {
                    listProduct.ItemsSource = products.Skip(++cbPage.SelectedIndex * 10).Take(10);
                }
            }
        }
        #endregion

        private void cbProducer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Tạo mới danh sách sản phẩm có tên chứa loại sản phẩm cần lọc
            ObservableCollection<PRODUCT> filterProducts = new ObservableCollection<PRODUCT>();
            if (cbProducer.SelectedIndex == -1)
            {
                reset_page(products);
                listProduct.ItemsSource = products;
            }
            else
            {
                switch (cbProducer.SelectedIndex)
                {
                    case 0:
                        {
                            reset_page(products);
                            listProduct.ItemsSource = products;
                        }
                        break;
                    case 1:
                        {
                            for (int i = 0; i < products.Count; i++)
                            {
                                if (products[i].PRODUCER.PRODUCER_NAME == "TESLA") // Nếu tìm thấy tên phù hợp
                                {
                                    filterProducts.Add(products[i]); // Thì thêm vào danh sách mới
                                }
                            }
                            reset_page(filterProducts);

                            listProduct.ItemsSource = filterProducts;
                        }
                        break;
                    case 2:
                        {
                            for (int i = 0; i < products.Count; i++)
                            {
                                if (products[i].PRODUCER.PRODUCER_NAME == "TOYOTA") // Nếu tìm thấy tên phù hợp
                                {
                                    filterProducts.Add(products[i]); // Thì thêm vào danh sách mới
                                }
                            }
                            reset_page(filterProducts);

                            listProduct.ItemsSource = filterProducts;
                        }
                        break;
                    case 3:
                        {
                            for (int i = 0; i < products.Count; i++)
                            {
                                if (products[i].PRODUCER.PRODUCER_NAME == "LAMBORGHINI") // Nếu tìm thấy tên phù hợp
                                {
                                    filterProducts.Add(products[i]); // Thì thêm vào danh sách mới
                                }
                            }
                            reset_page(filterProducts);

                            listProduct.ItemsSource = filterProducts;
                        }
                        break;
                    case 4:
                        {
                            for (int i = 0; i < products.Count; i++)
                            {
                                if (products[i].PRODUCER.PRODUCER_NAME == "LEXUS") // Nếu tìm thấy tên phù hợp
                                {
                                    filterProducts.Add(products[i]); // Thì thêm vào danh sách mới
                                }
                            }
                            reset_page(filterProducts);

                            listProduct.ItemsSource = filterProducts;
                        }
                        break;
                    case 5:
                        {
                            for (int i = 0; i < products.Count; i++)
                            {
                                if (products[i].PRODUCER.PRODUCER_NAME == "ROLLS-ROYCE") // Nếu tìm thấy tên phù hợp
                                {
                                    filterProducts.Add(products[i]); // Thì thêm vào danh sách mới
                                }
                            }
                            reset_page(filterProducts);

                            listProduct.ItemsSource = filterProducts;
                        }
                        break;
                    case 6:
                        {
                            for (int i = 0; i < products.Count; i++)
                            {
                                if (products[i].PRODUCER.PRODUCER_NAME == "MASERATI") // Nếu tìm thấy tên phù hợp
                                {
                                    filterProducts.Add(products[i]); // Thì thêm vào danh sách mới
                                }
                            }
                            reset_page(filterProducts);

                            listProduct.ItemsSource = filterProducts;
                        }
                        break;
                   case 7:
                        {
                            for (int i = 0; i < products.Count; i++)
                            {
                                if (products[i].PRODUCER.PRODUCER_NAME == "BUGATTI") // Nếu tìm thấy tên phù hợp
                                {
                                    filterProducts.Add(products[i]); // Thì thêm vào danh sách mới
                                }
                            }
                            reset_page(filterProducts);

                            listProduct.ItemsSource = filterProducts;
                        }
                        break;
                }
            }
        }
    }
}
