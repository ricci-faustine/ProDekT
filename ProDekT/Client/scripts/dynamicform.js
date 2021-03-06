﻿// inspired by : http://plnkr.co/edit/kqiheTEoGDQxAoQV3wxu?p=preview

var dynFormApp = angular.module('DynamicFormModule', []);

dynFormApp.controller('DynamicFormController', function ($scope, $log) {

    $scope.entity = {
        name: "Signup",
        fields:
          [
            {
                type: "text",
                name: "firstname",
                label: "Name",
                data: "",
                required_validation:
                {
                    required: true,
                    message: "Please give your firstname."
                }
            },
            {
                type: "number",
                name: "age",
                label: "Age",
                min: 18,
                max: 100,
                data: "",
                required_validation:
                {
                    required: true,
                    message: "Please enter your age."
                }
            },
            {
                type: "radio",
                name: "sex_id",
                label: "Sex",
                options: [
                    { id: 1, name: "Male" },
                    { id: 2, name: "Female" }
                ],
                data: "",
                required_validation:
                {
                    required: true,
                    message: "Please choose your sex."
                }
            },
            {
                type: "email",
                name: "emailUser",
                label: "Email",
                data: "",
                required_validation:
                {
                    required: true,
                    message: "Please give your Email."
                }
            },
            {
                type: "text",
                name: "hobbies", 
                label: "Hobbies", required: false, data: "",
                required_validation:
                {
                    required: false,
                    message: ""
                }
            },
            {
                type: "password",
                name: "pass",
                label: "Password",
                min: 6,
                max: 20,
                data: "",
                required_validation:
                {
                    required: true,
                    message: "Please enter a password for you to login to our site later."
                }
            },
            {
                type: "select",
                name: "city_id",
                label: "City",
                first_option_text: "Please choose your city",
                options: [
                    { name: "Newyork" },
                    { name: "Arizona" },
                    { name: "Washington" },
                    { name: "Florida" }
                ],
                data: "",
                required_validation:
                {
                    required: true,
                    message: "Please select your city."
                }
            },
            {
                type: "checkbox",
                name: "car_id",
                selected_items: {},
                label: "Preferred Car Makes",
                options: [
                    { id: 1, name: "BMW" },
                    { id: 2, name: "Audi" },
                    { id: 3, name: "Porche" },
                    { id: 4, name: "Jaguar" }
                ],
                data: "",
                required_validation:
                {
                    required: true,
                    message: "Please choose preferred car makes."
                }
            },
            {
                type: "textarea",
                rows: 10,
                cols: 50,
                name: "remarks",
                label: "Remarks",
                data: "",
                required_validation:
                {
                    required: true,
                    message: "Please enter remarks."
                }
            }
          ]
    };

    $scope.submitted = false;

    $scope.someSelected = function (object) {
        return Object.keys(object).some(function (key) {
            return object[key];
        });
    }

    $scope.submitForm = function () {
        $scope.submitted = true;


    }
})

  .directive("dynamicName", function ($compile) {
      return {
          restrict: "A",
          terminal: true,
          priority: 1000,
          link: function (scope, element, attrs) {
              element.attr('name', scope.$eval(attrs.dynamicName));
              element.removeAttr("dynamic-name");
              $compile(element)(scope);
          }
      }
  })
