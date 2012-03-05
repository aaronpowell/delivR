(function (exports, $, FileReader, JSON) {
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
        if (this.constructor !== exports.DelivR) {
            return new exports.DelivR(name);
        }

        var connection = $.connection(name),
            instance = this;

        connection.received(function (response) {
            var data;
            if (response.type === '') {
                data = new File(response.data);
            } else {
                data = {};
            }

            $(instance).trigger(response.type, data);
        });

        this.upload = function (file) {
            var reader = new FileReader();
            reader.onload = function () {
                var res = reader.result,
                    info = res.split(',');

                connection.send(JSON.stringify({
                    type: 'saveFile',
                    mimeType: file.type,
                    name: file.fileName,
                    content: info[1]
                }));
            };

            reader.readAsDataURL(file);
            return instance;
        };

        this.on = function (name, fn) {
            $(instance).on(name, fn);

            return instance;
        };

        connection.start();
    };
})(this, this.jQuery, FileReader, this.JSON);