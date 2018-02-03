window.addEventListener('load', function () {
    
    
})

app.makeRequest = function (url, data, method) {

    if (method === null || method === undefined) {
        method = 'POST';
    }

    let req = {
        url: url,
        type: method
    }
    
    if (data) {
        //req.data = JSON.stringify(data); 
        req.data = jQuery.param(data);
        //req.contentType = 'application/json; charset=utf-8';
        req.contentType = 'application/x-www-form-urlencoded; charset=utf-8';
    }

    return $.ajax(req);
}

app.isNullOrEmpty = function (str) {

    if (str === null || str === undefined) {
        return true;
    }

    if (typeof str !== 'string' && typeof str !== 'number') {

        return true;
    }

    let rgx = /^\s*$/;
    if (rgx.test(str)) {

        return true;
    }

    return false;
}