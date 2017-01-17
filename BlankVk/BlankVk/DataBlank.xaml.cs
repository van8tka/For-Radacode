using BlankVk.Model;

using Xamarin.Forms;

namespace BlankVk
{
    public partial class DataBlank : ContentPage
    {
        public DataBlank(ModelUser UserData)
        {
            InitializeComponent();
            UserNameData.Text ="Имя: "+ UserData.Name;
            UserSurNameData.Text ="Фамилия: "+ UserData.SurName;
            CountryData.Text = "Страна: "+ UserData.Country;
            CityData.Text = "Город: "+UserData.City;
            UniverData.Text = "ВУЗ: "+UserData.Univer;
        }
    }
}
