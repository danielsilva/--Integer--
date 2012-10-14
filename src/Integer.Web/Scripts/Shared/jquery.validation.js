$().ready(function () {
    var customErrorClass = 'input-validation-error';

    jQuery.extend(jQuery.validator.messages, {
        date: "inválida",
        email: "inválido",
        equalTo: "precisa ser igual",
        minlength: "selecione",
        required: "obrigatório"
    });
    jQuery.validator.setDefaults({
        errorElement: "span",
        validClass: 'input-validation-success',
        errorPlacement: function (error, element) {
            error.appendTo($(element.closest('form')).find("label[for=" + element.attr('id') + "]"));
            error.addClass('field-validation-error');
            element.addClass('input-validation-error');
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass(customErrorClass).removeClass(validClass);
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass(customErrorClass).addClass(validClass);
        }
    });
    jQuery.validator.addMethod("greaterThan",
                function (value, element, params) {
                    if (!/Invalid|NaN/.test(new Date(value))) {
                        return new Date(value) > new Date($(params).val());
                    }

                    return isNaN(value) && isNaN($(params).val())
                        || (Number(value) > Number($(params).val()));
                }, 'deve ser maior que início');
});