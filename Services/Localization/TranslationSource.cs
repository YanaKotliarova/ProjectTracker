using System.ComponentModel;
using System.Globalization;
using System.Resources;

namespace ProjectTracker.Services.Localization
{
    public class TranslationSource : INotifyPropertyChanged
    {
        private static readonly TranslationSource instance = new TranslationSource();

        public static TranslationSource Instance
        {
            get { return instance; }
        }

        private readonly ResourceManager resManager = Properties.Resources.ResourceManager;
        private CultureInfo currentCulture = null;

        public string this[string key]
        {
            get { return this.resManager.GetString(key, this.currentCulture); }
        }

        public List<string> GetLanguages()
        {
            List<string> languages = new List<string>();

            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
            foreach (CultureInfo culture in cultures)
            {
                ResourceSet rs = resManager.GetResourceSet(culture, true, false);
                if (rs != null)
                {
                    if (culture.Equals(CultureInfo.InvariantCulture))
                    {
                        continue;
                    }
                    else
                    {
                        languages.Add(culture.TwoLetterISOLanguageName);
                    }
                }
            }
            return languages;
        }

        public string SetLanguage()
        {
            CultureInfo cultureInfo = new CultureInfo(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);

            if (GetLanguages().Contains(cultureInfo.TwoLetterISOLanguageName))
                Instance.CurrentCulture = new CultureInfo(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);
            else Instance.CurrentCulture = new CultureInfo("en");

            return Instance.CurrentCulture.TwoLetterISOLanguageName;
        }

        public CultureInfo CurrentCulture
        {
            get { return this.currentCulture; }
            set
            {
                if (this.currentCulture != value)
                {
                    this.currentCulture = value;
                    var @event = this.PropertyChanged;
                    if (@event != null)
                    {
                        @event.Invoke(this, new PropertyChangedEventArgs(string.Empty));
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
