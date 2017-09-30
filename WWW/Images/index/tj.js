var SPM_AJAX_JSON = {
    trim: function(str) {
        return str.replace(/(^\s*)|(\s*$)/g, "");
    },
    getJSON: function(url, params, callbackFuncName, callback){
        var paramsUrl ="",
            jsonp = this.getQueryString(url)[callbackFuncName];
        for(var key in params){
            paramsUrl+="&"+key+"="+encodeURIComponent(params[key]);
        }
        url+=paramsUrl;
        window[jsonp] = function(data) {
            window[jsonp] = undefined;
            try {
                delete window[jsonp];
            } catch(e) {}

            if (head) {
                head.removeChild(script);
            }
            callback(data);
        };

        var head = document.getElementsByTagName('head')[0];
        var script = document.createElement('script');
        script.src = url;
        head.appendChild(script);
        return true;
    },
    getQueryString: function(url) {
        var result = {}, queryString = (url && url.indexOf("?")!=-1 && url.split("?")[1]) || location.search.substring(1),
            re = /([^&=]+)=([^&]*)/g, m;
        while (m = re.exec(queryString)) {
            result[decodeURIComponent(m[1])] = decodeURIComponent(m[2]);
        }
        return result;
    }
};
function writeCookieByDomainPath(a, c, b, d, e) {
 
var f = "";
 
b != null && (f = new Date((new Date).getTime() + b * 36E5), f = "; expires=" + f.toGMTString());
 
document.cookie = a + "=" + escape(c) + f + (e == null ? "": "; path=" + e) + (d == null ? "": ";domain=" + d)
 
}
 
var UrchinAplus = {
 
ysuid: "",
 
yuid: "",
 
lft: "",
 
ysid: "",
 
pvid: "",
 
rpvid: "",
 
ycid: "",
 
rycid: "",
 
ypstp: "",
 
yspstp: "",
 
yscnt: "",
 
ycms: "",
 
rcms: "",
 
unc: "",
 
frame: "",
 
ikuins: "",
 
dev: "",
 
mtype: "",
 
from: "",
 
abt: "",
 
ispay: "",
 
barrage: "",
 
cpid: "",
 
yvstp: "",
 
ysvstp: "",
 
urchinTracker: function() {
 
this._ySetLaifengCookie(this)
 
},

_yCookie: function(name) {
    var tmp, reg = new RegExp("(^| )"+name+"=([^;]*)(;|$)","gi");
		if((tmp = reg.exec(unescape(document.cookie)))){
			return tmp[2];
		}else{
			return null;	
	}
},
 
_yInfo: function() {
 
this.ycid = window.cateStr || "";
 
this.ycms = window.cateStr || "";
 
this.unc = !navigator.cookieEnabled ? 1 : 0;
 
this.frame = top.location != self.location ? 1 : 0;
 
this.ikuins = this._yGetIkuId();
 
this.dev = navigator.platform || "";
 
this.mtype = this._yGetMType();
 
this.from = this._yGetQueryString("from");
 
this.abt = this._yGetMType();
 
this.ispay = window.ispay || 0;
 
this.barrage = window.barrage || "";
 
this.cpid = window._stat_topics_cpid || "";
 
this.yvstp = parseInt(this._yCookie("__ayvstp")) || 0;
 
this.ysvstp = parseInt(this._yCookie("__aysvstp")) || 0;
this.yuid = this._yCookie("uk");
 
if (typeof goldlog != "undefined" && typeof goldlog.setPvExtdParam == "function") {
 
var a = "lft=" + this.lft;
 
a += "&ysid=" + this.ysid;
 
a += "&pvid=" + this.pvid;
 
a += "&rpvid=" + this.rpvid;
 
a += "&ycid=" + this.ycid;
 
a += "&rycid=" + this.rycid;
 
a += "&ypstp=" + this.ypstp;
 
a += "&yspstp=" + this.yspstp;
 
a += "&yscnt=" + this.yscnt;
 
a += "&ycms=" + this.ycms;
 
a += "&rcms=" + this.rcms;
 
a += "&unc=" + this.unc;
 
a += "&frame=" + this.frame;
 
a += "&ikuins=" + this.ikuins;
 
a += "&dev=" + this.dev;
 
a += "&mtype=" + this.mtype;
 
a += "&from=" + this.from;
 
a += "&abt=" + this.abt;
 
a += "&cpid=" + this.cpid;
goldlog.setPvExtdParam(a)
 
}
 
},
_yVvlogInfo: function() {
    var t = this;
    t.ysvstp = parseInt(t.ysvstp) + 1,
    t.yvstp = parseInt(t.yvstp) + 1;
    var e = {};
    return e.pc_i = t.ysuid,
    e.pc_u = t.yuid,
    e.lft = t.lft,
    e.seid = t.ysid,
    e.svstp = t.ysvstp,
    e.vsidc = t.yscnt,
    e.vstp = t.yvstp,
    e.pvid = t.pvid,
    e.rvpvid = t.rpvid,
    e.ycid = t.ycid,
    e.rycid = t.rycid,
    t._yResetVV(),
    e
},
_yResetVV: function() {
    var t = this;
    t._yCookie("__ayvstp", parseInt(t.yvstp)),
    t._yCookie("__aysvstp", parseInt(t.ysvstp), {
    expires: 2
    })
},
 
_yGetPvid: function(a) {
 
for (var c = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"], b = 0, d = "", e = new Date, b = 0; b < a; b++) {
 
var f = parseInt(Math.random() * Math.pow(10, 6)) % c.length;
 
d += c[f]
 
}
 
return e.getTime() + d
 
},
 
_yGetRPvid: function() {
 
try {
 
var a = this._yCookie("__arpvid") || "";
 
if (a == "") return "";
 
a = a.split("-");
 
return a[0] || ""
 
} catch(c) {
 
return ""
 
}
 
},
 
_yGetIkuId: function() {
 
var a = null;
 
if (navigator.userAgent.indexOf("MSIE") != -1 && window.ActiveXObject) try {
 
a = new ActiveXObject("iKuAgent.KuAgent2")
 
} catch(c) {}
 
if (a != void 0) return a.Youku_Hao;
 
return 0
 
},
 
_yGetMType: function() {
 
var a = "";
 
return a = navigator.userAgent.indexOf("Android") !== -1 ? "adr": navigator.userAgent.indexOf("iPad") !== -1 ? "ipa": navigator.userAgent.indexOf("iPhone") !== -1 ? "iph": navigator.userAgent.indexOf("iPod") !== -1 ? "ipo": "oth"
 
},
 
_yGetQueryString: function(a) {
 
a = window.location.search.substr(1).match(RegExp("(^|&)" + a + "=([^&]*)(&|$)"));
 
if (a != null) return unescape(a[2]);
 
return ""
 
},
 
_ySetLaifengCookie: function(a) {
 
var c = a._yGetPvid(6);
SPM_AJAX_JSON.getJSON("//lstat.youku.com/sck.php?pvid=" + c + "&jsoncallback=lftj",
 {},
 'jsoncallback',
function(b) {
var d = b.YOUKUSESSID,
 
e = b.CNA;
 
SPM_AJAX_JSON.trim(d) != "" && writeCookieByDomainPath("__ysuid", d, 8640, ".laifeng.com", "/");
 
SPM_AJAX_JSON.trim(e) != "" && writeCookieByDomainPath("cna", e, 8640, ".laifeng.com", "/");
 
a.ysuid= b.YOUKUSESSID,
 
a.lft = b.lft;
 
a.ysid = b.ysid;
 
a.pvid = c;
 
a.rpvid = b.rpvid;
 
a.ycid = window.cateStr || "";
 
a.rycid = b.rcid;
 
a.ypstp = b.pstp;
 
a.yspstp = b.spstp;
 
a.yscnt = b.scnt;
 
a._yInfo()
 
})
 
}
 
};
 
UrchinAplus.urchinTracker();
 
window.UrchinAplus = UrchinAplus;