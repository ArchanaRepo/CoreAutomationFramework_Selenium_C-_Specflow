using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoreAutomationFramework.Base
{
    public class Base
    {
        private readonly ParallelConfig parallelConfig;
        public Base(ParallelConfig parallelConfig)
        {
            this.parallelConfig = parallelConfig;
        }
        protected TPage GetInstance<TPage>() where TPage : BasePage,new()
        {
            return (TPage)Activator.CreateInstance(typeof(TPage));
        }
        public TPage As<TPage>() where TPage : BasePage
        {
            return (TPage)this;
        }
    }
}
