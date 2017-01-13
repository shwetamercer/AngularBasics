
var app = angular.module('myApp', ['ui.bootstrap']);

app.filter('filterByProperty', function () {
    /* array is first argument, each addiitonal argument is prefixed by a ":" in filter markup*/
    return function (dataArray, searchTerm, propertyName) {
        if (!dataArray) return;
        /* when term is cleared, return full array*/
        if (!searchTerm) {
            return dataArray
        } else {
            /* otherwise filter the array */
            var term = searchTerm.toLowerCase();
            return dataArray.filter(function (item) {
                return item[propertyName].toLowerCase().indexOf(term) > -1;
            });
        }
    }
});



app.filter('startFrom', function () {
    return function (input, start) {
        if (input) {
            start = +start;
            return input.slice(start);
        }
        return [];
    };
});

app.controller('PageCtrl', ['$scope', 'filterFilter', function ($scope, filterFilter) {
    $scope.header = ["ID", "BookName", "AuthorName", "ISBN"];

    $scope.items = [{ ID: "1", BookName: "Text Book 1", AuthorName: "Test Author 1", ISBN: "ZAO1" },
                           {ID:"2",BookName:"Text Book 2",AuthorName:"Test Author 2",ISBN:"SAU2"},
                           {ID:"3",BookName:"Text Book 3",AuthorName:"Test Author 3",ISBN:"SAU3"},
                           { ID: "4", BookName: "Text Book 4", AuthorName: "Test Author 4", ISBN: "SAU4" },
                            { ID: "5", BookName: "Text Book 5", AuthorName: "Test Author 5", ISBN: "SAU5" },
                           { ID: "6", BookName: "Text Book 6", AuthorName: "Test Author 6", ISBN: "SAU6" },
                           { ID: "7", BookName: "Text Book 7", AuthorName: "Test Author 7", ISBN: "SAU7" }

    ];

    // create empty search model (object) to trigger $watch on update
    $scope.search = {};

    $scope.resetFilters = function () {
        // needs to be a function or it won't trigger a $watch
        $scope.search = {};
    };

    // pagination controls
    $scope.currentPage = 1;
    $scope.totalItems = $scope.items.length;
    $scope.entryLimit = 3; // items per page
    $scope.noOfPages = Math.ceil($scope.totalItems / $scope.entryLimit);

    $scope.sortColumn = 'ID';
    
    $scope.toggleSort = function(index) {
        if($scope.sortColumn === $scope.header[index]){
            $scope.reverse = !$scope.reverse;
        }                    
        $scope.sortColumn = $scope.header[index];
    }

    // $watch search to update pagination
    $scope.$watch('search', function (newVal, oldVal) {
        $scope.filtered = filterFilter($scope.items, newVal);
        $scope.totalItems = $scope.filtered.length;
        $scope.noOfPages = Math.ceil($scope.totalItems / $scope.entryLimit);
        $scope.currentPage = 1;
    }, true);


}]);
