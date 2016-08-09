
define(function (require, exports, module) {
    var $ = require("../modules/jquery");
    var defaults = {
        'scrollTarget': $(window),
        'heightOffset': 0,
        'lock': false
    };
    var scrollPagination = function (options) {
        var opts = $.extend(defaults, options);
        var stopScroll = function () {
            $(document.body).attr('scrollPagination', 'disabled');
        };
        this.loadContent = function (callback) {
            var target = opts.scrollTarget;
            var mayLoadContent = $(target).scrollTop() + opts.heightOffset >= $(document).height() - $(target).height();
            if (mayLoadContent && !opts.lock) {
                callback.call(this, this);
            }
        };
        this.loadComplete = function () {
            console.log("lock is open");
            $(document.body).attr("scrollPagination", "enabled")
            opts.lock = false;
        }

        this.start = function () {
            $(document.body).attr("scrollPagination", "enabled")
            var target = opts.scrollTarget;
            $(target).scroll(function (event) {
                if ($(document.body).attr('scrollPagination') == 'enabled') {
                    loadContent();
                }
                else {
                   // event.stopPropagation();
                }
            });
        }

    }
    return {
        create: function (options) {
            return new scrollPagination(options);
        }
    };
});