Statistic = {
    createCORSRequest: function (method, url) {
        var xhr = new XMLHttpRequest();
        if ("withCredentials" in xhr) {

            // Check if the XMLHttpRequest object has a "withCredentials" property.
            // "withCredentials" only exists on XMLHTTPRequest2 objects.
            xhr.open(method, url, true);

        } else if (typeof XDomainRequest != "undefined") {

            // Otherwise, check if XDomainRequest.
            // XDomainRequest only exists in IE, and is IE's way of making CORS requests.
            xhr = new XDomainRequest();
            xhr.open(method, url);

        } else {

            // Otherwise, CORS is not supported by the browser.
            xhr = null;

        }
        return xhr;
    },
    MyIP: function () {
        var retorno = Statistic.createCORSRequest('GET', 'http://meuip.com/api/meuip.php');
        if (!retorno)
            throw new Error('CORS not supported');
        else {
            retorno.setRequestHeader('x-Trigger', 'CORS')
            retorno.send();
            retorno.onload = function (e) {
                return retorno.response;
            }
        }
    },

    getIP: function (callback) {
        function response(s) {
            callback(window.userip);

            s.onload = s.onerror = null;
            document.body.removeChild(s);
        }

        function trigger() {
            window.userip = false;

            var s = document.createElement("script");
            s.async = true;
            s.onload = function () {
                response(s);
            };
            s.onerror = function () {
                response(s);
            };

            s.src = "https://l2.io/ip.js?var=userip";
            document.body.appendChild(s);
        }

        if (/^(interactive|complete)$/i.test(document.readyState)) {
            trigger();
        } else {
            document.addEventListener('DOMContentLoaded', trigger);
        }

    }


    , Collect: function () {
        Statistic.getIP(function (ipr) {
            var model = StatisticModel;
            model.IP = ipr;
            model.Browser = bowser.name + ' ' + bowser.version + ' ' + bowser.osname;
            model.Page = location.pathname;

            console.log(model);
            Statistic.Send(model);

        });

    }

    , Send: function (data) {
        var xhttp = new XMLHttpRequest(); 
        xhttp.open('POST', 'http://localhost:53895/api/values', true);
        xhttp.setRequestHeader('Content-Type', 'application/json;charset=UTF-8');
        xhttp.setRequestHeader('X-PINGOTHER', 'pingpong');

        xhttp.withCredentials = false;
        //xhttp.onreadystatechange = handler;

        //xhttp.setRequestHeader("Authorization", "1234565");

        var input = JSON.stringify(data);
        xhttp.send(input);
    }


}
StatisticModel = {
    IP: null,
    Browser: null,
    Page: null,
    Parameters: null
}
