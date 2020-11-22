var app = angular.module('codeSampleApp', []); 

app.directive('numbersOnly', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attr, ngModelCtrl) {
            function fromUser(text) {
                if (text) {
                    var transformedInput = text.replace(/[^0-9]/g, '');

                    if (transformedInput !== text) {
                        //if (transformedInput && transformedInput.length == 0) {
                        //    transformedInput = 0;
                        //}
                        //alert(transformedInput);
                        //ngModelCtrl.$setValidity('parse', false);
                        ngModelCtrl.$setViewValue(transformedInput);
                        ngModelCtrl.$render();
                    }


                    return transformedInput;
                }
                return null;
            }

            ngModelCtrl.$parsers.push(fromUser);
        }
    };
});

app.controller('appController', function ($scope, $http) {
     
    $scope.resetForm = function (frm) {

        frm.$submitted = false;
        frm.$setPristine();
        angular.forEach(frm, function (input) {
            if (input && input.hasOwnProperty('$viewValue')) {
                input.$touched = false;
            }
        });

    }

})
