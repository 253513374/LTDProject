window.blazorAPI = {
    getOpenId: function () {
        return new Promise(function (resolve, reject) {
            var url = 'https://open.weixin.qq.com/connect/oauth2/authorize';
            var params = {
                appid: window.wxParams.appId,
                redirect_uri: window.wxParams.redirectUri,
                response_type: 'code',
                scope: window.wxParams.scope,
                state: window.wxParams.state
            };

            var urlParams = Object.keys(params).map(function (key) {
                return key + '=' + params[key];
            }).join('&');

            var fullUrl = url + '?' + urlParams + '#wechat_redirect';

            window.location.href = fullUrl;

            var code = getQueryVariable('code');

            if (code) {
                var data = {
                    code: code
                };

                $.ajax({
                    url: 'your api url',
                    data: data,
                    success: function (response) {
                        resolve(response.openid);
                    },
                    error: function (error) {
                        reject(error);
                    }
                });
            }
        });
    }
};

function getQueryVariable(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split('&');
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split('=');
        if (decodeURIComponent(pair[0]) == variable) {
            return decodeURIComponent(pair[1]);
        }
    }
    return undefined;
}