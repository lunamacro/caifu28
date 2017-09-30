using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace donet.io.rong.models {
    //rongUser.UserId, rongUser.Name, rongUser.PortraitUri
	
	public class RongUser {
        public string UserId { get; set; }
        public string Name { get; set; }

        public string PortraitUri { get; set; }

		public String toString() {
	    	return JsonConvert.SerializeObject(this);
	        }
		}
}
