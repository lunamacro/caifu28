function systemType() {
	var u = navigator.userAgent
	var isAndroid = u.indexOf('Android') > -1 || u.indexOf('Adr') > -1 //android终端
	var isiOS = !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/) //ios终端
	if(isAndroid) {
		//alert('是否是Android：' + isAndroid)
		return 'Android'
	} else if(isiOS) {
		//alert('是否是iOS：' + isiOS)
		return 'iOS'
	} else {
		return 'PC'
	}
}