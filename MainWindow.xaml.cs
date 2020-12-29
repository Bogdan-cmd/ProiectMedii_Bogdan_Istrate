using ProiectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
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

namespace ProiectMedii_Bogdan_Istrate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    enum ActionState
    {
        New,
        Edit,
        Delete,
        Nothing
    }
    public partial class MainWindow : Window
    {
        ActionState action = ActionState.Nothing;
        ProiectEntitiesModel ctx = new ProiectEntitiesModel();
        CollectionViewSource vinylViewSource;
        CollectionViewSource shopViewSource;
        CollectionViewSource customerViewSource;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            vinylViewSource =
    ((System.Windows.Data.CollectionViewSource)(this.FindResource("vinylViewSource")));
            shopViewSource =
                ((System.Windows.Data.CollectionViewSource)(this.FindResource("shopViewSource")));
            vinylViewSource.Source = ctx.Vinyls.Local;
            shopViewSource.Source = ctx.Shops.Local;
            customerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("customerViewSource")));
            customerViewSource.Source = ctx.Customers.Local;

            BindDataGrid();
            ctx.Vinyls.Load();
            ctx.Shops.Load();
            ctx.Customers.Load();

            cmbVinyle.ItemsSource = ctx.Vinyls.Local;
            cmbVinyle.SelectedValuePath = "VinylId";
            cmbShopuri.ItemsSource = ctx.Shops.Local;
            cmbShopuri.SelectedValuePath = "ShopId";
            
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Vinyl vinyl = null;
            if (action == ActionState.New)
            {
                try
                {

                    vinyl = new Vinyl()
                    {
                        Details = detailsTextBox.Text.Trim(),
                        Price = priceTextBox.Text.Trim()
                    };

                    ctx.Vinyls.Add(vinyl);
                    vinylViewSource.View.Refresh();
                    ctx.SaveChanges();
                }
                //using System.Data;
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnDelete.IsEnabled = true;
                btnCancel.IsEnabled = false;
                btnPrev.IsEnabled = true;
                btnNext.IsEnabled = true;
                detailsTextBox.IsEnabled = false;
                priceTextBox.IsEnabled = false;
                SetValidationBinding();

            }

            else if (action == ActionState.Edit)
            {
                try
                {
                    vinyl = (Vinyl)vinylDataGrid.SelectedItem;
                    vinyl.Details = detailsTextBox.Text.Trim();
                    vinyl.Price = priceTextBox.Text.Trim();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                vinylViewSource.View.Refresh();
                // pozitionarea pe item-ul curent
                vinylViewSource.View.MoveCurrentTo(vinyl);
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;

                btnPrev.IsEnabled = true;
                btnNext.IsEnabled = true;
                detailsTextBox.IsEnabled = false;
                priceTextBox.IsEnabled = false;
                SetValidationBinding();

            }

            else if (action == ActionState.Delete)
            {
                try
                {
                    vinyl = (Vinyl)vinylDataGrid.SelectedItem;
                    ctx.Vinyls.Remove(vinyl);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                vinylViewSource.View.Refresh();
                btnNew.IsEnabled = true;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = true;
                btnSave.IsEnabled = false;
                btnCancel.IsEnabled = false;

                btnPrev.IsEnabled = true;
                btnNext.IsEnabled = true;
                detailsTextBox.IsEnabled = false;
                priceTextBox.IsEnabled = false;
            }
            SetValidationBinding();
        }
        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            vinylViewSource.View.MoveCurrentToPrevious();

        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            vinylViewSource.View.MoveCurrentToNext();

        }


        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;

            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;

            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;
            detailsTextBox.IsEnabled = true;
            priceTextBox.IsEnabled = true;

            BindingOperations.ClearBinding(detailsTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(priceTextBox, TextBox.TextProperty);

            detailsTextBox.Text = "";
            priceTextBox.Text = "";

            Keyboard.Focus(detailsTextBox);
            SetValidationBinding();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;

            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;

            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;
            detailsTextBox.IsEnabled = true;
            priceTextBox.IsEnabled = true;

            BindingOperations.ClearBinding(detailsTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(priceTextBox, TextBox.TextProperty);

            Keyboard.Focus(detailsTextBox);
            SetValidationBinding();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;

            btnNew.IsEnabled = false;
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnSave.IsEnabled = true;
            btnCancel.IsEnabled = true;

            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;
            BindingOperations.ClearBinding(detailsTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(priceTextBox, TextBox.TextProperty);

        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;
            btnNew.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnEdit.IsEnabled = true;
            btnSave.IsEnabled = false;
            btnCancel.IsEnabled = false;

            btnPrev.IsEnabled = true;
            btnNext.IsEnabled = true;
            detailsTextBox.IsEnabled = false;
            priceTextBox.IsEnabled = false;

        }
        private void btnSave1_Click(object sender, RoutedEventArgs e)
        {
            Shop shop = null;
            if (action == ActionState.New)
            {
                try
                {
                    //instantiem Customer entity
                    shop = new Shop()
                    {
                        StockNumber = stockNumberTextBox.Text.Trim(),
                        Address = addressTextBox.Text.Trim()
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Shops.Add(shop);
                    shopViewSource.View.Refresh();
                    //salvam modificarile
                    //ctx.SaveChanges();
                }
                //using System.Data;
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                btnNew1.IsEnabled = true;
                btnEdit1.IsEnabled = true;
                btnSave1.IsEnabled = false;
                btnDelete1.IsEnabled = true;
                btnCancel1.IsEnabled = false;
                btnPrev1.IsEnabled = true;
                btnNext1.IsEnabled = true;
                stockNumberTextBox.IsEnabled = false;
                addressTextBox.IsEnabled = false;
            }

            else if (action == ActionState.Edit)
            {
                try
                {
                    shop = (Shop)shopDataGrid.SelectedItem;
                    shop.StockNumber = stockNumberTextBox.Text.Trim();
                    shop.Address = addressTextBox.Text.Trim();
                    //salvam modificarile
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                shopViewSource.View.Refresh();
                // pozitionarea pe item-ul curent
                shopViewSource.View.MoveCurrentTo(shop);
                btnNew1.IsEnabled = true;
                btnEdit1.IsEnabled = true;
                btnDelete1.IsEnabled = true;
                btnSave1.IsEnabled = false;
                btnCancel1.IsEnabled = false;

                btnPrev1.IsEnabled = true;
                btnNext1.IsEnabled = true;
                stockNumberTextBox.IsEnabled = false;
                addressTextBox.IsEnabled = false;
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    shop = (Shop)shopDataGrid.SelectedItem;
                    ctx.Shops.Remove(shop);
                    ctx.SaveChanges();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                shopViewSource.View.Refresh();
                btnNew1.IsEnabled = true;
                btnEdit1.IsEnabled = true;
                btnDelete1.IsEnabled = true;
                btnSave1.IsEnabled = false;
                btnCancel1.IsEnabled = false;

                btnPrev1.IsEnabled = true;
                btnNext1.IsEnabled = true;
                stockNumberTextBox.IsEnabled = false;
                addressTextBox.IsEnabled = false;
            }
            SetValidationBinding();

        }
        private void btnNew1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;
            btnNew1.IsEnabled = false;
            btnEdit1.IsEnabled = false;
            btnDelete1.IsEnabled = false;

            btnSave1.IsEnabled = true;
            btnCancel1.IsEnabled = true;

            btnPrev1.IsEnabled = false;
            btnNext1.IsEnabled = false;
            stockNumberTextBox.IsEnabled = true;
            addressTextBox.IsEnabled = true;

            BindingOperations.ClearBinding(stockNumberTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(addressTextBox, TextBox.TextProperty);
    
            stockNumberTextBox.Text = "";
            addressTextBox.Text = "";

            Keyboard.Focus(stockNumberTextBox);
        }
        private void btnEdit1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;

            btnNew1.IsEnabled = false;
            btnEdit1.IsEnabled = false;
            btnDelete1.IsEnabled = false;
            btnSave1.IsEnabled = true;
            btnCancel1.IsEnabled = true;

            btnPrev1.IsEnabled = false;
            btnNext1.IsEnabled = false;
            stockNumberTextBox.IsEnabled = true;
            addressTextBox.IsEnabled = true;
         
            BindingOperations.ClearBinding(stockNumberTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(addressTextBox, TextBox.TextProperty);

            Keyboard.Focus(stockNumberTextBox);
            SetValidationBinding();
        }

        private void btnDelete1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;

            btnNew1.IsEnabled = false;
            btnEdit1.IsEnabled = false;
            btnDelete1.IsEnabled = false;
            btnSave1.IsEnabled = true;
            btnCancel1.IsEnabled = true;

            btnPrev1.IsEnabled = false;
            btnNext1.IsEnabled = false;
            BindingOperations.ClearBinding(stockNumberTextBox, TextBox.TextProperty);
            BindingOperations.ClearBinding(addressTextBox, TextBox.TextProperty);
        }

        private void btnCancel1_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;

            btnNew1.IsEnabled = true;
            btnEdit1.IsEnabled = true;
            btnEdit1.IsEnabled = true;
            btnSave1.IsEnabled = false;
            btnCancel1.IsEnabled = false;

            btnPrev1.IsEnabled = true;
            btnNext1.IsEnabled = true;
            stockNumberTextBox.IsEnabled = false;
            addressTextBox.IsEnabled = false;
        }

        private void btnPrev1_Click(object sender, RoutedEventArgs e)
        {
            shopViewSource.View.MoveCurrentToPrevious();
        }

        private void btnNext1_Click(object sender, RoutedEventArgs e)
        {
            shopViewSource.View.MoveCurrentToNext();


        }
        private void btnSave2_Click(object sender, RoutedEventArgs e)
        {
            Customer customer = null;
            if (action == ActionState.New)
            {
                try
                {
                    Vinyl vinyl = (Vinyl)cmbVinyle.SelectedItem;
                    Shop shop = (Shop)cmbShopuri.SelectedItem;
                    //instantiem Order entity
                    customer = new Customer()
                    {

                        VinylId = vinyl.VinylId,
                        ShopId = shop.ShopId
                    };
                    //adaugam entitatea nou creata in context
                    ctx.Customers.Add(customer);
                    ctx.SaveChanges();
                    BindDataGrid();
                    customerViewSource.View.Refresh();
                    //salvam modificarile
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                btnNew2.IsEnabled = true;
                btnEdit2.IsEnabled = true;
                btnSave2.IsEnabled = false;
                btnDelete2.IsEnabled = true;
                btnCancel2.IsEnabled = false;
                btnPrev2.IsEnabled = true;
                btnNext2.IsEnabled = true;

                cmbVinyle.IsEnabled = false;
                cmbShopuri.IsEnabled = false;

            }
            else if (action == ActionState.Edit)
            {
                dynamic selectedCustomers = customerDataGrid.SelectedItem;
                try
                {
                    int current_id = selectedCustomers.OrderId;
                    var editedCustomers = ctx.Customers.FirstOrDefault(s => s.OrderId == current_id);
                    if (editedCustomers != null)
                    {
                        editedCustomers.VinylId = Int32.Parse(cmbVinyle.SelectedValue.ToString());
                        editedCustomers.ShopId = Convert.ToInt32(cmbShopuri.SelectedValue.ToString());
                        //salvam modificarile
                        ctx.SaveChanges();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                BindDataGrid();
                // pozitionarea pe item-ul curent
                customerViewSource.View.Refresh();
                vinylViewSource.View.MoveCurrentTo(selectedCustomers);

                btnNew2.IsEnabled = true;
                btnEdit2.IsEnabled = true;
                btnDelete2.IsEnabled = true;
                btnSave2.IsEnabled = false;
                btnCancel2.IsEnabled = false;

                btnPrev2.IsEnabled = true;
                btnNext2.IsEnabled = true;
                cmbVinyle.IsEnabled = false;
                cmbShopuri.IsEnabled = false;
            }
            else if (action == ActionState.Delete)
            {
                try
                {
                    dynamic selectedCustomers = customerDataGrid.SelectedItem;
                    int current_id = selectedCustomers.OrderId;
                    var deletedCustomers = ctx.Customers.FirstOrDefault(s => s.OrderId == current_id);
                    if (deletedCustomers != null)
                    {
                        ctx.Customers.Remove(deletedCustomers);
                        ctx.SaveChanges();
                        MessageBox.Show("Alegere stearsa Successfully", "Message");
                        BindDataGrid();
                    }
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                customerViewSource.View.Refresh();

                btnNew2.IsEnabled = true;
                btnEdit2.IsEnabled = true;
                btnDelete2.IsEnabled = true;
                btnSave2.IsEnabled = false;
                btnCancel2.IsEnabled = false;
                btnPrev2.IsEnabled = true;
                btnNext2.IsEnabled = true;

                cmbVinyle.IsEnabled = false;
                cmbShopuri.IsEnabled = false;
            }
        }

        private void btnNew2_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.New;

            btnNew2.IsEnabled = false;
            btnEdit2.IsEnabled = false;
            btnDelete2.IsEnabled = false;

            btnSave2.IsEnabled = true;
            btnCancel2.IsEnabled = true;

            btnPrev2.IsEnabled = false;
            btnNext2.IsEnabled = false;
            cmbVinyle.IsEnabled = true;
            cmbShopuri.IsEnabled = true;

            BindingOperations.ClearBinding(cmbVinyle, TextBox.TextProperty);
            BindingOperations.ClearBinding(cmbShopuri, TextBox.TextProperty);

            Keyboard.Focus(cmbVinyle);
        }

        private void btnEdit2_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Edit;

            btnNew2.IsEnabled = false;
            btnEdit2.IsEnabled = false;
            btnDelete2.IsEnabled = false;
            btnSave2.IsEnabled = true;
            btnCancel2.IsEnabled = true;

            btnPrev2.IsEnabled = false;
            btnNext2.IsEnabled = false;
            cmbVinyle.IsEnabled = true;
            cmbShopuri.IsEnabled = true;

            BindingOperations.ClearBinding(cmbVinyle, TextBox.TextProperty);
            BindingOperations.ClearBinding(cmbShopuri, TextBox.TextProperty);

            Keyboard.Focus(cmbVinyle);
        }

        private void btnDelete2_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Delete;

            btnNew2.IsEnabled = false;
            btnEdit2.IsEnabled = false;
            btnDelete2.IsEnabled = false;
            btnSave2.IsEnabled = true;
            btnCancel2.IsEnabled = true;

            btnPrev2.IsEnabled = false;
            btnNext2.IsEnabled = false;

            BindingOperations.ClearBinding(cmbVinyle, TextBox.TextProperty);
            BindingOperations.ClearBinding(cmbShopuri, TextBox.TextProperty);
        }

        private void btnCancel2_Click(object sender, RoutedEventArgs e)
        {
            action = ActionState.Nothing;

            btnNew2.IsEnabled = true;
            btnEdit2.IsEnabled = true;
            btnDelete2.IsEnabled = true;
            btnSave2.IsEnabled = false;
            btnCancel2.IsEnabled = false;

            btnPrev2.IsEnabled = true;
            btnNext2.IsEnabled = true;

            cmbVinyle.IsEnabled = false;
            cmbShopuri.IsEnabled = false;
        }

        private void btnPrev2_Click(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToPrevious();
        }

        private void btnNext2_Click(object sender, RoutedEventArgs e)
        {
            customerViewSource.View.MoveCurrentToNext();
        }
        private void BindDataGrid()
        {
            var queryCustomers = from cust in ctx.Customers
                               join vin in ctx.Vinyls on cust.OrderId equals
                               vin.VinylId
                               join sh in ctx.Shops on cust.ShopId
                   equals sh.ShopId
                               select new
                               {
                                   cust.OrderId,
                                   cust.VinylId,
                                   cust.ShopId,
                                   vin.Details,
                                   vin.Price,
                                   sh.StockNumber,
                                   sh.Address,
                               };
            customerViewSource.Source = queryCustomers.ToList();
        }
        private void SetValidationBinding()
        {

        }
    }
}