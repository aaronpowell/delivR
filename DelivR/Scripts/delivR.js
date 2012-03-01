(function (exports, $) {
    'use strict';
    var imageFilter = /^(image\/gif|image\/jpeg|image\/png|image\/svg\+xml|image\/tiff)/i;
    var File = function (data) {
        this.isImage = function () {
            return !!data.mimeType.match(imageFilter);
        };

        this.dataUri = function () {
            return 'data:' + data.mimeType + ';base64,' + data.content;
        };
    };

    exports.DelivR = function (name) {
        var connection = $.connection(name),
            instance = this;

        connection.received(function (data) {
            $(instance).trigger(data.type, new File(data.data));
        });

        this.upload = function (file) {

        };

        this.on = function (name, fn) {
            $(instance).on(name, fn);
        };

        connection.start();
    };
})(this, this.jQuery);