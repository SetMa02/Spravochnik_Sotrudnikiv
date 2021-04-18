using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace LABA1_MM.Model
{
    public class ListTitle : ObservableCollection<Title>
    {
        public ListTitle()
        {
            DbSet<Title> titles = PageEmployee.DataEntitiesEmployee.Titles;
            var queryTitle = from title in titles select title;
            foreach(Title titl in queryTitle)
            {
                this.Add(titl);
            }
        }
    }
}
