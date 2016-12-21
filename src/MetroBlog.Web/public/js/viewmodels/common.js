define(function (require, exports, module) {
    var jQuery = require("../modules/jquery");
    var layer = require("../modules/layer/layer");
    jQuery(document).ajaxStart(function (event) {
        config.load.show();
    }).ajaxComplete(function (event, xhr, settings) {
        config.load.hide();
    });

    var config = {
        layer: layer,
        msg: function (_msg, options) {
            options = options || { time: 700 };
            layer.msg(_msg, options);
        },
        load: {
            show: function () {
                if (!window.loadIndex) {
                    window.loadIndex = layer.load({
                        shade: [0.5, '#000'] //0.1透明度的白色背景
                    });
                }
            },

            hide: function (arg) {
                if (!arg && window.loadIndex) {
                    arg = window.loadIndex;
                    window.loadIndex = null;
                }
                layer.close(arg);
            }
        },
        confirm: function (content, options, yes, cancel) {
            layer.confirm(content, options, yes, cancel)
        },
        redirect: function (url) {
            window.location = url;
        }

    };
    return config;

});