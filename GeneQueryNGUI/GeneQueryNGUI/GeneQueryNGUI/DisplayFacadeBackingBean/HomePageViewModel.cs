using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneQueryNGUI.Pages;
using CommonMysql;

namespace GeneQueryNGUI.DisplayFacadeBackingBean
{
    public class HomePageViewModel : BaseDataBindingBean
    {
        private BullAction bullAction = new BullAction();
        private int allItemCount;
        private int currentItemCount;
        public Home view { get; set; }

        public HomePageViewModel()
        {
            allItemCount = bullAction.GetAllItemsInOberv().Count;
            currentItemCount = bullAction.GetAllCurrentItemsInOberv().Count;
        }

        public int AllItemCount
        {
            get
            {
                return allItemCount;
            }
            set
            {
                if (this.allItemCount != value)
                {
                    this.allItemCount = value;
                    OnPropertyChanged("AllItemCount");
                }
            }
        }

        public int CurrentItemCount
        {
            get
            {
                return currentItemCount;
            }
            set
            {
                if (this.currentItemCount != value)
                {
                    this.currentItemCount = value;
                    OnPropertyChanged("CurrentItemCount");
                }
            }
        }

    }
}
