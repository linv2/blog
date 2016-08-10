define(function (require, exports, module) {
    var Url = {
        Create: function () {
            var _href = window.location.href;
            var hash = '';
            var idx = _href.toString().indexOf("#");
            if (idx > 0) {
                hash = _href.toString().substr(idx + 1, _href.toString().length - idx - 1);
                _href = _href.toString().substr(0, idx);
            }
            var _queryString = '';
            var idx = _href.toString().indexOf("?");
            if (idx > 0) {
                _queryString = _href.toString().substr(idx + 1, _href.toString().length - idx - 1);
                _href = _href.toString().substr(0, idx);
            }
            _queryString = _queryString.split("&");
            var _url = {};
            _url.href = _href;
            _url.request = new Array();
            _url.hash = hash;

            _url.add = function (key, value) {
                if (key && key != '') {
                    if (value && typeof (value) != 'string') {
                        value = value.toString();
                    }
                    key = key.toString().toLocaleLowerCase();
                    this.request[key] = value
                }
            };
            for (var i = 0; i < _queryString.length; i++) {
                var tempValue = _queryString[i].split("=");
                var key = tempValue[0].toString().toLocaleLowerCase();
                _url.add(key, tempValue[1]);
            }
            _url.get = function (key) {
                return this.request[key];
            }
            _url.toString = function (_hash) {
                var query = '';
                for (k in this.request) {
                    var value = this.request[k];
                    if (value) {
                        query += k + "=" + encodeURI(value) + "&";
                    }
                }
                if (query != '' && query[query.length - 1] == '&') {
                    query = query.substr(0, query.length - 1);
                    query = this.href + "?" + query;
                } else {
                    query = this.href;
                }
                if (_hash && this.hash != '') {
                    query += "#" + this.hash;
                }
                return query;
            }
            _url.remove = function (key) {
                this.request[key] = null;
            }
            return _url;
        }
    };
    return Url.Create();
});