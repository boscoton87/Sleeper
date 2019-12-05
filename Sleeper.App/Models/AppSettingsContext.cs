using Sleeper.App.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sleeper.App.Models
{
    public class AppSettingsContext : IAppSettingsContext
    {
        public List<string> ChildControlNames { get; set; }

        public Dictionary<string, List<Action<string>>> ChildChangeEmitters { get; set; } = new Dictionary<string, List<Action<string>>>();

        private bool HibernateEnabled { get; set; }
        public string HibernateEnabledTextBox
        {
            get
            {
                return HibernateEnabled ? "Right" : "Left";
            }
            set
            {
                if (value == "Right")
                {
                    HibernateEnabled = true;
                }
                else
                {
                    HibernateEnabled = false;
                }
            }
        }

        private bool IsAdmin { get; set; }
        public string IsAdminTextBox
        {
            get
            {
                return IsAdmin.ToString();
            }
            set
            {
                bool isAdmin;
                if (bool.TryParse(value, out isAdmin))
                {
                    IsAdmin = isAdmin;
                }
            }
        }
    }
}
