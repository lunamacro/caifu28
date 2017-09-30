//《JavaScript权威指南》一书中有实现基于cookie的存储API，我把代码敲下来  
// 另外，书中的代码有错，以下为无BUG版并修改成1000天相当长的存储时间  
window.cookieStorage = (new(function() {
	var maxage = 60 * 60 * 24 * 1000;
	var path = '/';

	var cookie = getCookie();

	function getCookie() {
		var cookie = {};
		var all = document.cookie;
		if(all === "")
			return cookie;
		var list = all.split("; ");
		for(var i = 0; i < list.length; i++) {
			var cookies = list[i];
			var p = cookies.indexOf("=");
			var name = cookies.substring(0, p);
			var value = cookies.substring(p + 1);
			value = decodeURIComponent(value);
			cookie[name] = value;
		}
		return cookie;
	}

	var keys = [];
	for(var key in cookie)
		keys.push(key);

	this.length = keys.length;

	this.key = function(n) {
		if(n < 0 || n >= keys.length)
			return null;
		return keys[n];
	};

	this.setItem = function(key, value) {
		if(!(key in cookie)) {
			keys.push(key);
			this.length++;
		}

		cookie[key] = value;
		var cookies = key + "=" + encodeURIComponent(value);
		if(maxage)
			cookies += "; max-age=" + maxage;
		if(path)
			cookies += "; path=" + path;

		document.cookie = cookies;
	};

	this.getItem = function(name) {
		return cookie[name] || null;
	};

	this.removeItem = function(key) {
		if(!(key in cookie))
			return;

		delete cookie[key];

		for(var i = 0; i < keys.length; i++) {
			if(keys[i] === key) {
				keys.splice(i, 1);
				break;
			}
		}
		this.length--;

		document.cookie = key + "=; max-age=0";
	};

	this.clear = function() {
		for(var i = 0; i < keys.length; i++)
			document.cookie = keys[i] + "; max-age=0";
		cookie = {};
		keys = [];
		this.length = 0;
	};
})());

//本地存储，localStorage类没有存储空间的限制，而cookieStorage有存储大小限制
//在不支持localStorage的情况下会自动切换为cookieStorage  
window.myStorage = (new(function() {

	var storage; //声明一个变量，用于确定使用哪个本地存储函数  

	if(window.localStorage) {
		storage = localStorage; //当localStorage存在，使用H5方式  
	} else {
		storage = cookieStorage; //当localStorage不存在，使用兼容方式  
	}

	this.setItem = function(key, value) {
		storage.setItem(key, value);
	};

	this.getItem = function(name) {
		return storage.getItem(name);
	};

	this.removeItem = function(key) {
		storage.removeItem(key);
	};

	this.clear = function() {
		storage.clear();
	};
})());