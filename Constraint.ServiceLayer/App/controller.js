
(function (angular) {
    'use strict';
    var constraintApp = angular.module('app', [
        'ngRoute',
        'ui.bootstrap'
    ]);
    constraintApp.controller('archiveConstraintCtrl', ['$scope', '$filter', '$location', '$http', function ($scope, $filter, $location, $http) {
        $scope.archiveConstraintList = [];
        $scope.markedList = [];
        $scope.deletedValue = {};
        $scope.persons = [];
        $scope.teams = [];
        $scope.reasons = [];
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.choseAll = true;
        $scope.uploading = false;
        $scope.isUsingDateFilter = true;
        $scope.startDate = new Date().toISOString().substr(0, 10);
        $scope.endDate = new Date().toISOString().substr(0, 10);

        /*Veritabanindaki tum arsiv tablosunu getirir.Items parametresine atar.
        Ilk basta isMarked degeri false olarak belirlenir sec butonunu buna bagli yaptigim icin,
        zaten arsivden cikarirken otomatik isaretlenmise gidecegi icin isMarked true olmalidir.*/
        $scope.fetchData = function () {
            $scope.archiveConstraintList = [];
            $scope.markedList = [];
            $http.get("/ArchiveConstraint/GetConstraint").then(function (response) {
                var i = 0;
                $scope.archiveConstraintList = response.data.map((x) => {
                    return { ...x, isMarked: false, isDelayEntered: true, id: ++i };
                });
                $scope.uploading = true;

            }, function () {
                $scope.uploading = true;
            });
        };
        /**
        *Items listesindeki isMarked===true olduğu kisimlari markedListesine atıyorum.
        *Listedeki her bir elemani Kisit Tablosuna geri ekliyorum.
        *markedListesini sifirliyorum.
        */
        $scope.sendToMarked = function () {
            if ($scope.archiveConstraintList == null || $scope.archiveConstraintList.length == 0)
                toastr.warning("Tablo Boş");
            else {
                $scope.markedList = $scope.archiveConstraintList.filter((t) => t.isMarked === true);
                if ($scope.markedList.length !== 0) {
                    $http.post(`/ArchiveConstraint/SendToMarked`, $scope.markedList, $scope.markedList).then(function (response) {
                        if (response.data) {
                            toastr.success("Başarılı!");
                            if ($scope.isUsingDateFilter) $scope.fetchDataFromDate();
                            else $scope.fetchData();
                        }
                        else toastr.error("Tekrar Deneyiniz.");
                    }, function () {
                        toastr.error("Bir şeyler ters gitti.");
                    })
                }
            }
        };
        $scope.delete = function (item) {
            $http.post('/ArchiveConstraint/Delete/' + item.constraintID).then(function (response) {
                if (response.data) {
                    $http.post('/DelayHistory/Delete/' + item.delayID).then(function (response) {
                        if (response.data) {
                            if ($scope.isUsingDateFilter) $scope.fetchDataFromDate();
                            else $scope.fetchData();
                            toastr.success("Kalıcı olarak silindi")
                        }
                    }, function () {
                        toastr.error("Tekrar Deneyiniz.");
                    })
                }
            }, function () {
                toastr.error("Tekrar Deneyiniz.");
            })
        };
        $scope.deleteAll = function () {
            if ($scope.archiveConstraintList == null || $scope.archiveConstraintList.length == 0)
                toastr.warning("Tablo Boş");
            else
                $http.get('/ArchiveConstraint/DeleteAll').then(function (response) {
                    if (response.data) {
                        if ($scope.isUsingDateFilter) $scope.fetchDataFromDate();
                        else $scope.fetchData();
                        toastr.success("Arşiv sıfırlandı.")
                    }
                }, function () {
                    toastr.error("Tekrar Deneyiniz.");
                })
        };
        /**
        * Arsiv tablosundan silerken emin misiniz diye sordurttugum method
        */
        $scope.deleteMethod = function (item) {
            $scope.deletedValue = item;
        };
        /**
        * Tum arsiv tablosunu exportlar
        */
        $scope.Export = function () {
            if ($scope.archiveConstraintList != null && $scope.archiveConstraintList.length !== 0) {
                $scope.uploading = false;
                $http({
                    method: "get",
                    url: '/ArchiveConstraint/Export',
                    responseType: "blob"
                }).then(function (response) {
                    $scope.uploading = true;
                    var date = new Date().toISOString().substr(0, 10);
                    //var myBlob = new Blob([result.data], {type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'})
                    const url = window.URL.createObjectURL(response.data);
                    const link = document.createElement("a");
                    link.href = url;
                    link.setAttribute("download", `ArsivlenmisKisitlar//${date}.xlsx`); //or any other extension
                    document.body.appendChild(link);
                    link.click();
                    toastr.success("Excel Dosyasına Aktarıldı.");
                });
            } else {
                toastr.warning("Tablo Boş");
            }
        };
        /**
        * Iki tarih arasındaki exportlama methodur.
        */
        $scope.ExportByDate = function () {
            if ($scope.archiveConstraintList != null && $scope.archiveConstraintList.length !== 0 && $scope.isUsingDateFilter === true) {
                $scope.uploading = false;
                $http({
                    method: "get",
                    url: `/ArchiveConstraint/ExportByDate?startDate=${$scope.startDate}&endDate=${$scope.endDate}`,
                    responseType: "blob"
                }).then(function (response) {
                    $scope.uploading = true;
                    var date = new Date().toISOString().substr(0, 10);
                    //var myBlob = new Blob([result.data], {type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'})
                    const url = window.URL.createObjectURL(response.data);
                    const link = document.createElement("a");
                    link.href = url;
                    link.setAttribute("download", `ArsivlenmisKisitlar//${date}.xlsx`); //or any other extension
                    document.body.appendChild(link);
                    link.click();
                    toastr.success("Excel Dosyasına Aktarıldı.");
                });
            } else {
                toastr.warning("Tablo Boş");
            }
        };
        $scope.setDateFilter = function () {
            if (document.getElementById("data-endDate").value == null || document.getElementById("data-startDate").value == null)
                return;
            $scope.endDate = document.getElementById("data-endDate").value;
            $scope.startDate = document.getElementById("data-startDate").value;
        };
        /**
          * Iki tarih arasindaki veriyi veritabanindan cagiran methottur.
        */
        $scope.fetchDataFromDate = function () {
            $scope.setDateFilter();
            $scope.markedList = [];
            $http({
                method: "get",
                url: `/ArchiveConstraint/GetByDate?startDate=${$scope.startDate}&endDate=${$scope.endDate}`,
            }).then(function (response) {
                var i = 0;
                $scope.archiveConstraintList = response.data.map((x) => {
                    return { ...x, isMarked: false, id: ++i };
                });
                $scope.isUsingDateFilter = true;
            })
        };
        $scope.sortBy = function (column) {
            $scope.sortColumn = column;
            $scope.reverse = !$scope.reverse;
        };
    }])
    constraintApp.controller('companyTeamCtrl', ['$scope', '$filter', '$location', '$http', function ($scope, $filter, $location, $http) {
        $scope.companyTeam = [];
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;


        $scope.getData = function () {
            $http.get('/CompanyTeam/GetManager').then(function (response) {
                $scope.$watch('searchText', function (term) {
                    $scope.companyTeam = $filter('filter')(response.data, term);
                    var i = 0;
                    $scope.companyTeam = $scope.companyTeam.map(x => { return { ...x, id: ++i } });
                });

            }, function () {
                alert("Error Occur");
            })
        };

        $scope.Delete = function (i) {
            $http.post('/CompanyTeam/Delete/' + i.companyID).then(function (result) {
                if (result.data) {
                    toastr.success(i.companyName + " silindi.");
                    $scope.getData();
                }
            })
        };

        $scope.sortBy = function (column) {
            $scope.sortColumn = column;
            $scope.reverse = !$scope.reverse;
        };
        $scope.InsertDataTeam = function () {
            if ($scope.companyName != null && $scope.companyName != "") {
                $scope.companyTeamDTO = {};
                $scope.companyTeamDTO.companyName = $scope.companyName;
                $scope.companyTeamDTO.companyCode = "-";

                $http.post('/CompanyTeam/Create', $scope.companyTeamDTO).then(function (result) {
                    if (result != null) {
                        toastr.success($scope.companyName + " eklendi.");
                        $scope.companyName = "";
                        $scope.companyCode = "";
                        $scope.getData();
                    }
                })
            }
            else toastr.warning("Ekip adı giriniz.");

        };
        $scope.InsertDataCompany = function () {
            if (($scope.companyName != null && $scope.companyCode != null && $scope.companyName != "" && $scope.companyCode != "")) {
                $scope.companyTeamDTO = {};
                $scope.companyTeamDTO.companyName = $scope.companyName;
                $scope.companyTeamDTO.companyCode = $scope.companyCode;
                $http.post('/CompanyTeam/Create', $scope.companyTeamDTO).then(function (result) {
                    if (result != null) {
                        toastr.success($scope.companyName + " eklendi.");
                        $scope.companyName = "";
                        $scope.companyCode = "";
                        $scope.getData();
                    }
                })
            }
            else toastr.warning("Firma adını ve kodunu girdiğinizden emin olunuz!");
        };
        $scope.UpdateData = function () {
            $scope.companyTeamDTO = {};
            $scope.companyTeamDTO.companyID = $scope.companyIDUpdate;
            $scope.companyTeamDTO.companyName = $scope.companyNameUpdate;
            $scope.companyTeamDTO.companyCode = $scope.companyCodeUpdate;
            if ($scope.companyNameUpdate != null && $scope.companyNameUpdate != "") {
                $http.post('/CompanyTeam/Edit', $scope.companyTeamDTO).then(function (result) {
                    if (result != null) {
                        toastr.success($scope.companyTeamDTO.companyName + " düzenlendi.");
                        $scope.companyIDUpdate = "";
                        $scope.companyNameUpdate = "";
                        $scope.companyCodeUpdate = "";
                        $scope.getData();
                    }
                })
            }

        };
        $scope.UpdateDataH = function (i) {
            $scope.companyIDUpdate = i.companyID;
            $scope.companyNameUpdate = i.companyName;
            $scope.companyCodeUpdate = i.companyCode;
        };
    }])
    constraintApp.controller('constraintEntryCtrl', ['$scope', '$filter', '$location', '$http', function ($scope, $filter, $location, $http) {
        //Veritabanindaki kisit tablosundaki veriler girilen materialCode göre getirilir. Bu arrayde saklanır. 
        $scope.constraintList = [];
        //Hat Üstü olanlari gizle/göster icin gerekli. Gizlemeden once constraintList listesi burada saklaniyor.
        $scope.storeConstraintList = [];
        //Veritabınındaki arşivlenmemis otelemeler getiriliyor bu listeye.
        $scope.delayHistoryList = [];
        //Kisit giris tablosuna veriler bu arrayden getiriliyor. 
        $scope.checkedConstraint = [];
        //DB' deki isimleri, takımları,firmaları,oteleme sebeplerini bu arraylerde sakliyorum.
        $scope.persons = [];
        $scope.teams = [];
        $scope.reasons = [];
        $scope.person = "";
        $scope.team = "";
        $scope.reason = "";
        //Pagination icin gerekli veriler.
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;
        $scope.totalDelayAmount = 0;
        $scope.enteredDelayAmount = 0;
        //Oteleme gecmislerini gizle/goster butonu icin toggle olusturuluyor.
        $scope.allExpanded = true;
        //Hat ustlerini gizle/goster butonu icin toggle olusturuluyor.
        $scope.aboveLineExpanded = true;
        //kisit giriş tablosu gosterilsin gosterilmesin icin
        $scope.displayConstraintTable = false;
        //veriler getirilirken ekranda bekleme olusturabilmek icin (spinner)
        $scope.uploading = true;
        $scope.uploadingFirst = true;
        //Gizlenen Hat ustlerini bu arrayde tutuyorum.
        $scope.hidedAboveLineList = [];
        //Gizle/göster Hat ustleri icin tutulan toogle.
        $scope.toggleAboveLine = true;
        //update Butonuna tıklanınca update edilecek veriyi bunda saklıyorum.
        $scope.updateDelay = {};
        $scope.blockControl = true;
        $scope.searchMaterial = "";
        $scope.controlIsBlock = function () {
            $scope.uploadingFirst = false;
            $scope.constraintList = [];
            $scope.checkedConstraint = [];
            $http.get('/User/HasPermissionConstraint?hasPermissionConstraint=' + false).then(function (response) {
                if (response.data) {
                    $scope.searchMaterial = "Yöneticiniz Tarafından Kitlenmiştir.Kısıt Giremezsiniz.";
                    $scope.blockControl = true;
                } else {
                    $scope.blockControl = false;
                }
                $scope.uploadingFirst = true;
            });
        };
        //ekleme silme yada guncelleme islemlerinden sonra veriler refresh yaptıgım db ile esitledigim nokta.

        $scope.fetch = function () {
            $scope.uploading = false;
            $http.get('/DelayHistory/GetManager').then(function (response) {
                $scope.delayHistoryList = response.data.filter(
                    (t) => t.isArchive === false
                );
                $scope.delayHistoryList = $scope.delayHistoryList.map((x) => {
                    return { ...x, constraintDelayID: "" };
                });

                $scope.storeConstraintList = [];
                $scope.toggleAboveLine = true;
                $http.get('/ConstraintEntry/GetMaterial?material=' + $scope.searchMaterial).then(function (response) {
                    var i = 0;
                    $scope.toogleConstraintTable();
                    $scope.constraintList = response.data.map((x) => {
                        return { ...x, _hide: true, _checked: false, _delayList: [], _isAboveLine: false, id: ++i };
                    });

                    $scope.constraintList.map(
                        (x) => (
                            (x.delayAmount = 0),
                            (x.companyTeam = ""),
                            (x.delayReason = ""),
                            (x.chargePerson = ""),
                            (x.delayDate = null),
                            (x.delayDetail = "")
                        )
                    );
                    $scope.constraintList.map(x => ($scope.delayHistoryList.map(t => (t.productCode === x.productCode && t.boundMontageID === x.boundMontageID) ? x._delayList.push(t) : true, x.aboveLine == "Hat Üstü" ? x._isAboveLine = true : x._isAboveLine = false)));
                    $scope.constraintList.map(x => x._delayList = x._delayList.map(t => { return { ...t, constraintID: x.constraintID, materialCode: x.materialCode, materialText: x.materialText, plannedDate: x.plannedDate, amount: x.amount, customer: x.customer, constraintDelayID: x.delayID, version: x.version, aboveLine: x.aboveLine, mip: x.mip, tob: x.tob, treeAmount: x.treeAmount, madeDate: x.dateCurrent } }));
                    console.log($scope.constraintList)
                    $scope.uploading = true;
                }, function () {
                    $scope.uploading = true;
                    toastr.error("Yüklenemedi.");
                });
            }, function () {
                $scope.uploading = true;
                toastr.error("Yüklenemedi.");
            })
        }
        $scope.ClickSearchButton = function () {
            $scope.uploading = false;
            $scope.searchMaterial = document.getElementById("data-material-input").value.trim();
            $scope.toogleConstraintTable();
            if ($scope.searchMaterial !== "" || $scope.searchMaterial !== undefined) {
                $http.get('/DelayHistory/GetManager').then(function (response) {
                    $scope.delayHistoryList = response.data.filter(
                        (t) => t.isArchive === false
                    );
                    $scope.delayHistoryList = $scope.delayHistoryList.map((x) => {
                        return { ...x, constraintDelayID: "" };
                    });
                    $scope.fetchReasons();
                    $scope.fetchTeams();
                    $scope.fetchPersons();
                    $scope.fetchData();
                }, function () {
                    $scope.uploading = true;
                    toastr.error("Yüklenemedi.");
                })
            }

        };
        $scope.fetchData = function () {
            $scope.searchMaterialText = "";
            $scope.uploading = false;
            if ($scope.searchMaterial !== "") {
                $scope.checkedConstraint = [];
                $scope.storeConstraintList = [];
                $scope.hidedAboveLineList = [];
                $scope.totalDelayAmount = 0;
                $scope.enteredDelayAmount = 0;
                $scope.inputBoxDelayAmount = 0;
                $http.get('/ConstraintEntry/GetMaterial?material=' + $scope.searchMaterial).then(function (response) {
                    $scope.toogleConstraintTable();
                    var i = 0;
                    $scope.constraintList = response.data.map((x) => {
                        return { ...x, _hide: true, _checked: false, _delayList: [], _isAboveLine: false, id: ++i };
                    });
                    $scope.constraintList.map(
                        (x) => (
                            (x.delayAmount = 0),
                            (x.companyTeam = ""),
                            (x.delayReason = ""),
                            (x.chargePerson = ""),
                            (x.delayDate = null),
                            (x.delayDetail = "")
                        )
                    );
                    if ($scope.constraintList.length !== 0)
                        $scope.searchMaterialText = $scope.constraintList[0].materialText;
                    $scope.constraintList.map(x => ($scope.delayHistoryList.map(t => (t.productCode === x.productCode && t.boundMontageID === x.boundMontageID) ? x._delayList.push(t) : true, x.aboveLine == "Hat Üstü" ? x._isAboveLine = true : x._isAboveLine = false)));
                    $scope.constraintList.map(x => x._delayList = x._delayList.map(t => { return { ...t, constraintID: x.constraintID, materialCode: x.materialCode, materialText: x.materialText, plannedDate: x.plannedDate, amount: x.amount, customer: x.customer, constraintDelayID: x.delayID, version: x.version, aboveLine: x.aboveLine, mip: x.mip, tob: x.tob, treeAmount: x.treeAmount, madeDate: x.dateCurrent } }));
                    
                    $scope.uploading = true;
                }, function () {
                    $scope.uploading = true;
                    toastr.error("Yüklenemedi.");
                });
            }
        };
        $scope.sortBy = function (column) {
            $scope.sortColumn = column;
            $scope.reverse = !$scope.reverse;
        };
        $scope.sortByConstraint = function (column) {
            $scope.sortColumn1 = column;
            $scope.reverse1 = !$scope.reverse1;
        };
        $scope.ExpandAll = function () {
            $scope.allExpanded = !$scope.allExpanded;
            $scope.constraintList.map(
                (x) => (
                    (x._hide = $scope.allExpanded)
                )
            );
        };

        $scope.fetchReasons = function () {
            if ($scope.searchMaterial !== "") {
                $http.get('/PostponementReason/GetManager').then(function (response) {
                    $scope.reasons = response.data.map((x) => x.delayName);
                }, function () {
                    toastr.error("Yüklenemedi.");
                })
            }
        };
        $scope.fetchPersons = function () {
            if ($scope.searchMaterial !== "") {
                $http.get('/User/GetManager').then(function (response) {
                    $scope.persons = response.data.map((x) => x.userName);
                }, function () {
                    toastr.error("Yüklenemedi.");
                })
            }
        };
        $scope.fetchTeams = function () {
            if ($scope.searchMaterial !== "") {
                $http.get('/CompanyTeam/GetManager').then(function (response) {
                    $scope.teams = response.data.map((x) => x.companyName);
                }, function () {
                    toastr.error("Yüklenemedi.");
                })
            }
        };

        $scope.toogleConstraintTable = function () {
            if ($scope.checkedConstraint.length === 0) {
                $scope.displayConstraintTable = false;
            } else {
                $scope.displayConstraintTable = true;
            }
        };
        $scope.setCheckBox = function (item) {
            var foundIndex = $scope.findItemIndex(item);
            $scope.constraintList[foundIndex]._checked = !item._checked;
        };
        $scope.findItemIndex = function (item) {
            return $scope.constraintList.findIndex((i) => i.constraintID === item.constraintID);
        };
        $scope.getCurrentDate = function (item) {
            item.dateCurrent = new Date().toISOString().slice(0, 10);
            $http.get('/Security/GetUserName').then(function (response) {
                item.chargePerson = response.data;
            })
        };
        $scope.addTable = async function (addedItem) {
            await control();
            $scope.getCurrentDate(addedItem);
            $scope.setCheckBox(addedItem);
            addedItem.delayCode = addedItem.materialCode;
            await $scope.controlCheckedConstraint(addedItem);
            await $scope.toogleConstraintTable();
        };
        $scope.controlCheckedConstraint = async function (temp) {
            var index = $scope.checkedConstraint.indexOf(temp);
            if (index === -1) {
                await $scope.checkedConstraint.push(temp);
                $scope.totalDelayAmount += parseInt(temp.amount);
                //$scope.restOfDelayAmount=$scope.totalDelayAmount-$scope.enteredDelayAmount;
            } else {
                $scope.totalDelayAmount -= parseInt(temp.amount);
                $scope.enteredDelayAmount -= parseInt(temp.delayAmount);
                $scope.checkedConstraint.splice(index, 1);
            }
            await control();
        };
        async function control() {
            await $scope.checkedConstraint.sort(compare);
        };
        function compare(a, key) {
            const bandB = parseInt(key.id);
            const bandA = parseInt(a.id);
            let comparison = 0;
            if (bandA > bandB) {
                comparison = 1;
            } else if (bandA < bandB) {
                comparison = -1;
            }
            return comparison;
        };
        $scope.save = function (item) {
            item.delayDate = document.getElementById("data-delay").value;
            item.dateCurrent = document.getElementById("data-current").value;
            if (item.delayAmount === undefined) {
                toastr.warning("Öteleme Miktarını giriniz!");
                return;
            }
            if (parseInt(item.delayAmount) > parseInt(item.amount)) {
                toastr.warning("Öteleme miktarı planlanan miktardan fazla olamaz!");
                return;
            }

            if (parseInt(item.delayAmount) <= 0) {
                toastr.warning("Öteleme miktarı sıfır yada sıfırdan küçük olamaz!");
                return;
            }
            if (item.delayReason === null || item.delayReason === "") {
                toastr.warning("Sebep giriniz.");
                return;
            }
            if (item.companyTeam === null || item.companyTeam === "") {
                toastr.warning("Firma-Takım giriniz.");
                return;
            }
            if (item.chargePerson === null || item.chargePerson === "") {
                toastr.warning("Sorumlu birey giriniz.");
                return;
            }
            if (item.delayDate === null || item.delayDate.length === 0) {
                toastr.warning("Öteleme tarihini giriniz.");
                return;
            }
            $scope.postDelay(item);
        };
        $scope.postDelay = function (item) {
            if (item.delayDetail === null || item.delayDetail.length === 0)
                item.delayDetail = "-";
            var data = {
                productCode: item.productCode,
                delayCode: item.delayCode,
                delayAmount: item.delayAmount,
                delayDate: item.delayDate,
                delayReason: item.delayReason,
                delayDetail: item.delayDetail,
                companyTeam: item.companyTeam,
                chargePerson: item.chargePerson,
                madeDate: item.dateCurrent,
                boundConstraintID: item.constraintID,
                boundMontageID: item.boundMontageID,
            };
            $http.post('/DelayHistory/Create', data).then(function (response) {

                $scope.updateConstraint(response.data.delayID, item);
            }, function () {
                toastr.error("Tekrar Deneyiniz!");
            })
        };
        $scope.updateConstraint = function (id, item) {
            item.constraintDelayID = id;
            var list = {
                isMarked: false,
                isDelayEntered: true,
                constraintID: item.constraintID,
                materialCode: item.materialCode,
                materialText: item.materialText,
                productCode: item.productCode,
                plannedDate: item.plannedDate,
                amount: item.amount,
                customer: item.customer,
                version: item.version,
                delayID: id,
                delayCode: item.delayCode,
                delayAmount: item.delayAmount,
                delayDate: item.delayDate,
                delayReason: item.delayReason,
                delayDetail: item.delayDetail,
                companyTeam: item.companyTeam,
                chargePerson: item.chargePerson,
                dateCurrent: item.dateCurrent,
                aboveLine: item.aboveLine,
                treeAmount: item.treeAmount,
                mip: item.mip,
                tob: item.tob,
            };
            $http.post('/ConstraintEntry/Edit', list).then(function (response) {
                item._checked = false;
                $scope.enteredDelayAmount -= item.delayAmount;
                item.delayAmount = 0;
                item.chargePerson = "";
                item.delayDetail = "";
                item.delayDate = null;
                item.delayReason = "";
                item.companyTeam = "";
                $scope.getCurrentDate(item);
                $scope.controlCheckedConstraint(item);
                $scope.toogleConstraintTable();
                $scope.fetch();
                toastr.success("Kayıt Başarılı");
            }, function () {
                toastr.error("Tekrar Deneyiniz!");
            })

        };
        $scope.cancel = function (item) {
            $scope.controlCheckedConstraint(item);
            item._checked = false;
            $scope.toogleConstraintTable();
        };
        $scope.copy = function () {
            var temp = $scope.checkedConstraint[0];
            if (temp != null) {
                $scope.checkedConstraint.map((x) => {
                    (x.delayReason = temp.delayReason),
                        (x.delayDetail = temp.delayDetail),
                        (x.delayDate = document.getElementById("data-delay").value),
                        (x.dateCurrent = document.getElementById("data-current").value),
                        (x.companyTeam = temp.companyTeam),
                        (x.chargePerson = temp.chargePerson);
                });
            }
        };

        $scope.seperatePlanDate = function () {
            if ($scope.checkedConstraint.length !== 0) {
                $scope.enteredDelayAmount = 0;
                $scope.checkedConstraint.map((x) => {
                    (x.delayAmount = x.amount),
                        ($scope.enteredDelayAmount += parseInt(x.delayAmount));
                });
            }
        };
        $scope.saveAll = function () {
            if ($scope.contolAllDelayAmountBeforeSave()) {
                $http.post('/ConstraintEntry/SaveAll', $scope.checkedConstraint).then(function (response) {
                    if (response.data) {
                        $scope.checkedConstraint = [];
                        $scope.totalDelayAmount = 0;
                        $scope.enteredDelayAmount = 0;
                        $scope.fetch();
                        toastr.success("Kayıt Başarılı");
                    }
                    else {
                        toastr.error("Kayıt Başarısız!");
                    }

                }, function () {
                    toastr.error("Tekrar Deneyiniz!");
                })
            }

        };
        $scope.contolAllDelayAmountBeforeSave = function () {
            for (var i = 0; i < $scope.checkedConstraint.length; i++) {
                var item = $scope.checkedConstraint[i];
                if (
                    parseInt(item.delayAmount) > parseInt(item.amount) ||
                    item.delayAmount < 1
                ) {
                    toastr.warning("Öteleme miktarlarını kontrol ediniz.");

                    return false;
                }
                if (item.delayReason === null || item.delayReason === "") {

                    toastr.warning("Sebep giriniz.");
                    return false;
                }
                if (item.companyTeam === null || item.companyTeam === "") {

                    toastr.warning("Firma-Takım giriniz.");
                    return false;
                }
                if (item.chargePerson === null || item.chargePerson === "") {

                    toastr.warning("Sorumlu birey giriniz.");
                    return false;
                }
                if (item.delayDate === null || item.delayDate.length === 0) {

                    toastr.warning("Öteleme tarihini giriniz.");
                    return false;
                }
            }
            return true;
        };
        $scope.changeEnteredDelayAmount = async function (item) {
            if (item.delayAmount === undefined) {
                item.delayAmount = 0;
                return;
            }
            var value = parseInt(item.delayAmount);
            var valueTemp = parseInt(item.amount);
            if (
                value < valueTemp &&
                parseInt(item.delayAmount) >= 0 &&
                $scope.enteredDelayAmount === $scope.totalDelayAmount
            ) {
                $scope.enteredDelayAmount +=
                    parseInt(item.delayAmount) - parseInt(item.amount);
                return;
            }
            else if (value > valueTemp) {
                await toastr.warning("Öteleme miktarını planlanandan daha az girmelisiniz.");
                item.delayAmount = 0;
            }
            await $scope.calculateEntered();
        };
        $scope.calculateEntered = function () {
            if ($scope.checkedConstraint.length !== 0) {
                $scope.enteredDelayAmount = 0;
                $scope.checkedConstraint.map((x) => {
                    x.delayAmount <= x.amount
                        ? ($scope.enteredDelayAmount += parseInt(x.delayAmount))
                        : (x.delayAmount = 0);
                });
            }
        };
        $scope.divideForFaraway = function () {
            $scope.inputBoxDelayAmount = document.getElementById("data-amount-input").value;
            if ($scope.inputBoxDelayAmount >= 0) {
                $scope.enteredDelayAmount = 0;
                $scope.checkedConstraint.map((x) => (x.delayAmount = 0));
                for (var i = $scope.checkedConstraint.length - 1; i >= 0; i--) {
                    var item = $scope.checkedConstraint[i];
                    if ($scope.inputBoxDelayAmount > parseInt(item.amount)) {
                        item.delayAmount = item.amount;
                        $scope.inputBoxDelayAmount -= parseInt(item.amount);
                        $scope.enteredDelayAmount += parseInt(item.amount);
                    } else {
                        item.delayAmount = $scope.inputBoxDelayAmount;
                        $scope.enteredDelayAmount += parseInt($scope.inputBoxDelayAmount);
                        break;
                    }
                }
            }
            document.getElementById("data-amount-input").value = 0;

        };
        $scope.divideForNear = function () {
            $scope.inputBoxDelayAmount = document.getElementById("data-amount-input").value;
            if ($scope.inputBoxDelayAmount >= 0) {
                $scope.enteredDelayAmount = 0;
                $scope.checkedConstraint.map((x) => (x.delayAmount = 0));
                for (var i = 0; i < $scope.checkedConstraint.length; i++) {
                    var item = $scope.checkedConstraint[i];
                    if ($scope.inputBoxDelayAmount > parseInt(item.amount)) {
                        item.delayAmount = item.amount;
                        $scope.inputBoxDelayAmount -= parseInt(item.amount);
                        $scope.enteredDelayAmount += parseInt(item.amount);
                    } else {
                        item.delayAmount = $scope.inputBoxDelayAmount;
                        //$scope.inputBoxDelayAmount -= parseInt(item.delayAmount);
                        $scope.enteredDelayAmount += parseInt($scope.inputBoxDelayAmount);
                        break;
                    }
                }
            }
            document.getElementById("data-amount-input").value = 0;

        };
        $scope.hideAboveLine = function () {

            if ($scope.checkedConstraint.length === 0) {
                if ($scope.toggleAboveLine) {
                    $scope.storeConstraintList = $scope.constraintList;
                    $scope.constraintList = $scope.constraintList.filter((x) => (x._isAboveLine === false));
                }
                else {
                    $scope.constraintList = $scope.storeConstraintList;
                }
                $scope.toggleAboveLine = !$scope.toggleAboveLine
            } else { toastr.warning("Kısıt Giriş Tablosunda Çalışıyorsunuz. Daha sonra tekrar deneyiniz."); }

        };
        $scope.toogleDelay = function (item) {
            $scope.updateDelay = item;
        };
        $scope.updateDelayHistory = function (item) {
            item.delayDate = document.getElementById("data-delayModal").value;
            item.madeDate = document.getElementById("data-currentModal").value;
            $scope.updateDelayFromModal(item);

            if (item.delayID === item.constraintDelayID) {
                $scope.updateConraintFromModal(item);
            }
        };
        $scope.updateDelayFromModal = function (delay) {
            if (delay.delayDetail === null || delay.delayDetail.length === 0)
                delay.delayDetail = "-";
            var data = {
                isMarked: delay.isMarked,
                isArchive: delay.isArchive,
                delayID: delay.delayID,
                productCode: delay.productCode,
                delayCode: delay.delayCode,
                delayAmount: delay.delayAmount,
                delayDate: delay.delayDate,
                delayReason: delay.delayReason,
                delayDetail: delay.delayDetail,
                companyTeam: delay.companyTeam,
                chargePerson: delay.chargePerson,
                madeDate: delay.madeDate,
                boundConstraintID: delay.boundConstraintID,
                boundMontageID: delay.boundMontageID,
            };
            $http.post('/DelayHistory/Edit', data).then(function (response) {
                $scope.toogleConstraintTable();
                //$scope.fetch();
                if (delay.delayID !== delay.constraintDelayID) {

                    toastr.success("Başarılı.");
                }
            }, function () {
                toastr.error("Tekrar Deneyiniz!");
            })
        };
        $scope.updateConraintFromModal = function (constraintData) {
            constraintData.isMarked = false;
            constraintData.isDelayEntered = true;
            constraintData.delayID = constraintData.delayID;
            constraintData.dateCurrent = constraintData.madeDate;
            $http.post('/ConstraintEntry/Edit', constraintData).then(function (response) {
                $scope.toogleConstraintTable();
                toastr.success("Güncelleme Başarılı");
            }, function () {
                toastr.error("Tekrar Deneyiniz! updateConraintFromModal");
            })
        };

        $scope.deleteDelayHistory = function (item) {
            $scope.deleteDelayFromModal(item);
            if (item.delayID === item.constraintDelayID) {
                $scope.deleteConraintFromModal(item);
            }
        };
        $scope.deleteDelayFromModal = function (item) {
            $http.post('/DelayHistory/Delete/' + item.delayID).then(function (response) {
                if (item.delayID !== item.constraintDelayID) {
                    toastr.success("Silindi");
                    $scope.fetch();
                }
                $scope.toogleConstraintTable();

            }, function () {
                toastr.error("Tekrar Deneyiniz!");
            })
        };
        $scope.deleteConraintFromModal = function (constraintData) {
            var list = {
                isMarked: false,
                isDelayEntered: false,
                constraintID: constraintData.constraintID,
                materialCode: constraintData.materialCode,
                materialText: constraintData.materialText,
                productCode: constraintData.productCode,
                plannedDate: constraintData.plannedDate,
                amount: constraintData.amount,
                customer: constraintData.customer,
                version: constraintData.version,
                delayID: "",
                delayCode: "",
                delayAmount: "",
                delayDate: "",
                delayReason: "",
                delayDetail: "",
                companyTeam: "",
                chargePerson: "",
                dateCurrent: "",
                aboveLine: constraintData.aboveLine,
                treeAmount: constraintData.treeAmount,
                mip: constraintData.mip,
                tob: constraintData.tob,
            };
            $http.post('/ConstraintEntry/Edit', list).then(function (response) {
                if (response.data) {
                    toastr.success("Silindi");
                    $scope.fetch();
                    $scope.toogleConstraintTable();
                } else
                    toastr.error("Tekrar Deneyiniz!");
            }, function () {
                toastr.error("Tekrar Deneyiniz!");
            })
        };
    }])
    constraintApp.controller('constraintMarkedCtrl', ['$scope', '$filter', '$location', '$http', function ($scope, $filter, $location, $http) {
        $scope.constraintList = [];
        $scope.archiveList = [];
        $scope.archiveConstraintList = [];
        $scope.updateDelay = {};
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.choseAll = true;
        $scope.uploading = true;



        $scope.fetchData = function () {
            $scope.uploading = false;
            $scope.constraintList = [];
            $scope.archiveList = [];
            $http.get('/ConstraintEntry/GetMarkedList').then(function (response) {
                var i = 0;
                $scope.constraintList = response.data;
                $scope.constraintList = $scope.constraintList.map((x) => {
                    return { ...x, isMarked: false, id: ++i }
                });
                $scope.uploading = true;
            }, function () {
                toastr.error("Yüklenemedi.");
            })
        };
        $scope.getCurrentDate = function (item) {
            item.dateCurrent = new Date().toISOString().slice(0, 10);
        };
        $scope.sortBy = function (column) {
            $scope.sortColumn = column;
            $scope.reverse = !$scope.reverse;
        };
        $scope.chosenAll = function () {
            if ($scope.constraintList.length !== 0) {
                if ($scope.choseAll)
                    $scope.constraintList.map((x) => {
                        x.isMarked = true
                    });

                else
                    $scope.constraintList.map((x) => {
                        x.isMarked = false
                    });
                $scope.choseAll = !$scope.choseAll;
            }
        };

        $scope.Export = function () {
            if ($scope.constraintList.length !== 0) {
                $scope.uploading = false;
                $http({
                    method: "get",
                    url: '/ConstraintEntry/Export?isMark=true',
                    responseType: "blob"
                }).then(function (response) {
                    var date = new Date().toISOString().substr(0, 10);
                    //var myBlob = new Blob([result.data], {type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'})
                    const url = window.URL.createObjectURL(response.data);
                    const link = document.createElement("a");
                    link.href = url;
                    link.setAttribute("download", `IsaretlenmisKisitlar//${date}.xlsx`); //or any other extension
                    document.body.appendChild(link);
                    link.click();
                    $scope.uploading = true;
                    toastr.success("Excel Dosyasına Aktarıldı.");
                });
            } else {
                toastr.error("Tablo Boş");
            }
        };
        $scope.sendToArchive = function () {
            $scope.archiveList = $scope.constraintList.filter(
                (t) => t.isMarked === true && t.isDelayEntered === true
            );

            if ($scope.archiveList.length !== 0) {
                $scope.archiveConstraint();
            }
        };
        $scope.archiveConstraint = function () {
            $scope.convertMeetingData($scope.archiveList);
        };
        $scope.convertMeetingData = function (item) {
            var meetingList = [];
            var meetingTeamList = [];
            var delayHistory = [];
            for (var i = 0; i < item.length; i++) {
                var listTeam = {
                    constraintID: item[i].constraintID,
                    materialCode: item[i].materialCode,
                    materialText: item[i].materialText,
                    productCode: item[i].productCode,
                    plannedDate: item[i].plannedDate,
                    amount: item[i].amount,
                    customer: item[i].customer,
                    version: item[i].version,
                    delayID: item[i].delayID,
                    delayCode: item[i].delayCode,
                    delayAmount: item[i].delayAmount,
                    delayDate: item[i].delayDate,
                    delayReason: item[i].delayReason,
                    delayDetail: item[i].delayDetail,
                    companyTeam: item[i].companyTeam,
                    chargePerson: item[i].chargePerson,
                    dateCurrent: item[i].dateCurrent,
                };
                var listCompany = {
                    constraintID: item[i].constraintID,
                    materialCode: item[i].materialCode,
                    materialText: item[i].materialText,
                    productCode: item[i].productCode,
                    plannedDate: item[i].plannedDate,
                    amount: item[i].amount,
                    customer: item[i].customer,
                    version: item[i].version,
                    delayID: item[i].delayID,
                    delayCode: item[i].delayCode,
                    delayAmount: item[i].delayAmount,
                    delayDate: item[i].delayDate,
                    delayReason: item[i].delayReason,
                    delayDetail: item[i].delayDetail,
                    companyTeam: item[i].companyTeam,
                    chargePerson: item[i].chargePerson,
                    dateCurrent: item[i].dateCurrent,
                    changedAmount: 1,
                    companyTeamCode: item[i].companyTeamCode,
                };
                var listDelay = {
                    isMarked: true,
                    isArchive: true,
                    delayID: item[i].delayID,
                    productCode: item[i].productCode,
                    delayCode: item[i].delayCode,
                    delayAmount: item[i].amount,
                    delayDate: item[i].delayDate,
                    delayReason: item[i].delayReason,
                    delayDetail: item[i].delayDetail,
                    companyTeam: item[i].companyTeam,
                    chargePerson: item[i].chargePerson,
                    madeDate: item[i].dateCurrent,
                    boundConstraintID: item[i].constraintID,
                    boundMontageID: item[i].boundMontageID,
                };
                var archiveConstraint = {
                    constraintID: item[i].constraintID,
                    materialCode: item[i].materialCode,
                    materialText: item[i].materialText,
                    productCode: item[i].productCode,
                    plannedDate: item[i].plannedDate,
                    amount: item[i].amount,
                    customer: item[i].customer,
                    version: item[i].version,
                    delayID: item[i].delayID,
                    delayCode: item[i].delayCode,
                    delayAmount: item[i].delayAmount,
                    delayDate: item[i].delayDate,
                    delayReason: item[i].delayReason,
                    delayDetail: item[i].delayDetail,
                    companyTeam: item[i].companyTeam,
                    chargePerson: item[i].chargePerson,
                    dateCurrent: item[i].dateCurrent,
                    aboveLine: item[i].aboveLine,
                };
                meetingList.push(listCompany);
                meetingTeamList.push(listTeam);
                delayHistory.push(listDelay);
                $scope.archiveConstraintList.push(archiveConstraint);
            }
            $http.post('/ConstraintEntry/TransferToMeeting', meetingList).then(function (response) {
                $http.post('/ConstraintEntry/TransferToMeetingTeam', meetingTeamList).then(function (response) {
                    toastr.success("Toplantı saylarına aktarıldı.")
                }, function () {

                })
            }, function () {

            })


            $scope.updateDelayHistory(delayHistory);
        };
        $scope.updateDelayHistory = function (item) {
            $http.post('/DelayHistory/EditList', item).then(function (response) {
                $scope.addArchiveConstraint($scope.archiveConstraintList);
            }, function () {

            })
        };
        $scope.addArchiveConstraint = function (item) {
            $http.post('/ArchiveConstraint/CreateAll', item).then(function (response) {
                $scope.deleteConstraintArchive(item);
            }, function () {

            })
        };
        $scope.deleteConstraintArchive = function (item) {
            $http.post('/ConstraintEntry/DeleteAll', item).then(function (response) {
                if (response.data) {
                    toastr.success("Seçilenler arşivlendi.");
                    $scope.fetchData();
                }
            }, function () {
                toastr.error("Tekrar Deneyiniz!");

            })
        };
        $scope.sendToArchiveWithoutMeeting = function () {
            $scope.archiveList = $scope.constraintList.filter(
                (t) => t.isMarked === true && t.isDelayEntered === true
            );
            var item = $scope.archiveList;
            if ($scope.archiveList.length !== 0) {
                var delayHistory = [];
                for (var i = 0; i < item.length; i++) {

                    var listDelay = {
                        isMarked: true,
                        isArchive: true,
                        delayID: item[i].delayID,
                        productCode: item[i].productCode,
                        delayCode: item[i].delayCode,
                        delayAmount: item[i].amount,
                        delayDate: item[i].delayDate,
                        delayReason: item[i].delayReason,
                        delayDetail: item[i].delayDetail,
                        companyTeam: item[i].companyTeam,
                        chargePerson: item[i].chargePerson,
                        madeDate: item[i].dateCurrent,
                        boundConstraintID: item[i].constraintID,
                        boundMontageID: item[i].boundMontageID,
                    };
                    var archiveConstraint = {
                        constraintID: item[i].constraintID,
                        materialCode: item[i].materialCode,
                        materialText: item[i].materialText,
                        productCode: item[i].productCode,
                        plannedDate: item[i].plannedDate,
                        amount: item[i].amount,
                        customer: item[i].customer,
                        version: item[i].version,
                        delayID: item[i].delayID,
                        delayCode: item[i].delayCode,
                        delayAmount: item[i].delayAmount,
                        delayDate: item[i].delayDate,
                        delayReason: item[i].delayReason,
                        delayDetail: item[i].delayDetail,
                        companyTeam: item[i].companyTeam,
                        chargePerson: item[i].chargePerson,
                        dateCurrent: item[i].dateCurrent,
                        aboveLine: item[i].aboveLine,
                    };
                    delayHistory.push(listDelay);
                    $scope.archiveConstraintList.push(archiveConstraint);
                }
                $scope.updateDelayHistory(delayHistory);
            }
        }
        $scope.updateConstraintForNoMarked = function (item) {
            item.isMarked = false;
            item.isDelayEntered = true;
            $http.post('/ConstraintEntry/Edit', item).then(function (response) {
                $scope.fetchData();
                toastr.success("Başarıyla Çıkarıldı!");
            }, function () {

            })
        };
        $scope.deleteDelay = function (item) {
            $http.post('/DelayHistory/Delete/' + item.delayID).then(function (response) {
                if (response.data) {
                    $scope.deleteConstraint(item);
                }
            }, function () {
                toastr.error("Tekrar Deneyiniz.");
            })
        };
        $scope.deleteConstraint = function (item) {
            item.isMarked = false;
            item.isDelayEntered = false;
            item.delayID = "";
            item.delayCode = "";
            item.delayAmount = "";
            item.delayDate = "";
            item.delayReason = "";
            item.delayDetail = "";
            item.companyTeam = "";
            item.chargePerson = "";
            item.dateCurrent = "";
            $http.post('/ConstraintEntry/Edit', item).then(function (response) {
                toastr.success("Başarıyla Silindi!");
                $scope.fetchData();
            }, function () {
                toastr.error("Tekrar Deneyiniz.");
            })
        };

    }])
    constraintApp.controller('constraintNoMarkedCtrl', ['$scope', '$filter', '$location', '$http', function ($scope, $filter, $location, $http) {
        $scope.constraintList = [];
        $scope.markedList = [];
        $scope.updateDelay = {};
        $scope.persons = [];
        $scope.teams = [];
        $scope.reasons = [];
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.choseAll = true;
        $scope.uploading = true;


        $scope.fetchData = function () {
            $scope.constraintList = [];
            $scope.uploading = false;
            $http.get('/ConstraintEntry/GetNoMarkedList').then(function (response) {
                var i = 0;
                $scope.constraintList = response.data.map((x) => {
                    return { ...x, id: ++i };
                });
                $scope.uploading = true;
            }, function () {
                toastr.error("Yüklenemedi.");
            })
        };
        $scope.getCurrentDate = function (item) {
            item.dateCurrent = new Date().toISOString().slice(0, 10);
        };
        $scope.sortBy = function (column) {
            $scope.sortColumn = column;
            $scope.reverse = !$scope.reverse;
        };
        $scope.fetchReasons = function () {
            if ($scope.searchMaterial !== "") {
                $http.get('/PostponementReason/GetManager').then(function (response) {
                    $scope.reasons = response.data.map((x) => x.delayName);
                }, function () {
                    toastr.error("Yüklenemedi.");
                })
            }
        };
        $scope.fetchPersons = function () {
            if ($scope.searchMaterial !== "") {
                $http.get('/User/GetManager').then(function (response) {
                    $scope.persons = response.data.map((x) => x.userName);
                }, function () {
                    toastr.error("Yüklenemedi.");
                })
            }
        };
        $scope.fetchTeams = function () {
            if ($scope.searchMaterial !== "") {
                $http.get('/CompanyTeam/GetManager').then(function (response) {

                    $scope.teams = response.data.map((x) => x.companyName);
                }, function () {
                    toastr.error("Yüklenemedi.");
                })
            }
        };
        $scope.chosenAll = function () {
            if ($scope.constraintList.length !== 0) {
                if ($scope.choseAll)
                    $scope.constraintList.map((x) => {
                        x.isMarked = true
                    });

                else
                    $scope.constraintList.map((x) => {
                        x.isMarked = false
                    });
                $scope.choseAll = !$scope.choseAll;
            }
        };

        $scope.Export = function () {
            if ($scope.constraintList.length !== 0) {
                $scope.uploading = false;
                $http({
                    method: "get",
                    url: '/ConstraintEntry/Export?isMark=false',
                    responseType: "blob"
                }).then(function (response) {
                    $scope.uploading = true;
                    var date = new Date().toISOString().substr(0, 10);
                    //var myBlob = new Blob([result.data], {type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'})
                    const url = window.URL.createObjectURL(response.data);
                    const link = document.createElement("a");
                    link.href = url;
                    link.setAttribute("download", `IsaretlenmemisKisitlar//${date}.xlsx`); //or any other extension
                    document.body.appendChild(link);
                    link.click();
                    toastr.success("Excel Dosyasına Aktarıldı.");
                });
            } else {
                toastr.error("Tablo Boş");
            }
        };
        $scope.sendToMarked = function () {
            $scope.markedList = $scope.constraintList.filter(
                (t) => t.isMarked === true && t.isDelayEntered === true
            );

            if ($scope.markedList.length !== 0) {
                $scope.updateConstraintForMarked();
            }
        };
        $scope.updateConstraintForMarked = function () {
            $http.post('/ConstraintEntry/EditList', $scope.markedList).then(function (response) {
                toastr.success("Başarıyla İşaretlendi.");
                if (response.data) {
                    $scope.fetchData();
                }

            }, function () {

            })
        };
        $scope.toogleConstraint = function (item) {

            $scope.updateDelay = item;

            $scope.fetchPersons();
            $scope.fetchReasons();
            $scope.fetchTeams();
        };
        $scope.updateConstraint = function (item) {
            item.isMarked = false;
            item.isDelayEntered = true;
            $http.post('/ConstraintEntry/Edit', item).then(function (response) {

            }, function () {

            })
        };
        $scope.updateMethod = function (item) {
            if (parseInt(item.delayAmount) > parseInt(item.amount)) {
                toastr.error("Öteleme miktarı planlanan miktardan fazla olamaz!");
                return;
            }
            if (parseInt(item.delayAmount) <= 0) {
                toastr.error("Öteleme miktarı sıfır yada sıfırdan küçük olamaz!");
                return;
            }
            item.delayDate = document.getElementById("data-delay").value;
            item.dateCurrent = document.getElementById("data-current").value;
            $scope.updateConstraint(item);
            $scope.updateDelayHistory(item);
        };
        $scope.updateDelayHistory = function (item) {
            //oteleme geçmişi varsa silsin/editlesin yoksa da silsin.Önce varmı diye bakalım varsa editlesin/silsin yoksa hiçbir şey yapmasın.
            item.isMarked = false;
            $http.get('/DelayHistory/Details/' + item.delayID).then(function (response) {
                if (response.data) {
                    var data = {
                        delayID: item.delayID,
                        productCode: item.productCode,
                        delayCode: item.delayCode,
                        delayAmount: item.delayAmount,
                        delayDate: item.delayDate,
                        delayReason: item.delayReason,
                        delayDetail: item.delayDetail,
                        companyTeam: item.companyTeam,
                        chargePerson: item.chargePerson,
                        madeDate: item.dateCurrent,
                        boundConstraintID: item.constraintID,
                        boundMontageID: item.boundMontageID,
                    };
                    $http.post('/DelayHistory/Edit', data).then(function (response) {
                        toastr.success('Güncellleme Başarılı');
                        //$scope.fetchData();
                    }, function () {
                        toastr.error("Tekrar Deneyiniz!");
                    })
                }
                else
                    toastr.success('Güncellleme Başarılı');
            }, function () {
                toastr.error("Tekrar Deneyiniz!");

            })

        };
        $scope.deleteConstraint = function (item) {
            item.isMarked = false;
            item.isDelayEntered = false;
            item.delayID = "";
            item.delayCode = "";
            item.delayAmount = "";
            item.delayDate = "";
            item.delayReason = "";
            item.delayDetail = "";
            item.companyTeam = "";
            item.chargePerson = "";
            item.dateCurrent = "";
            $http.post('/ConstraintEntry/Edit', item).then(function (response) {
                toastr.success("Başarıyla Silindi!");
                $scope.fetchData();
            }, function () {
                toastr.error("Tekrar Deneyiniz!");
            })
        };
        $scope.deleteDelay = function (item) {
            $http.get('/DelayHistory/Details/' + item.delayID).then(function (response) {
                if (response.data) {
                    $http.post('/DelayHistory/Delete/' + item.delayID).then(function (response) {
                        if (response.data) {

                        }
                    }, function () {
                        toastr.error("Tekrar Deneyiniz!");
                    })
                }
                $scope.deleteConstraint(item);
            }, function () {
                toastr.error("Tekrar Deneyiniz!");

            })

        };
    }])
    constraintApp.controller('excelMontageCtrl', ['$scope', '$filter', '$location', '$http', function ($scope, $filter, $location, $http) {

        $scope.uploading = true;
        var formdata = new FormData();
        $scope.getTheFiles = function ($files) {
            angular.forEach($files, function (value, key) {
                formdata.append(key, value);
            });
        };

        // NOW UPLOAD THE FILES.
        $scope.uploadFiles = function () {
            $scope.uploading = false;
            var request = {
                method: 'POST',
                url: '/MontagePlan/Upload',
                data: formdata,
                headers: {
                    'Content-Type': undefined
                }
            };


            $http(request)
                .then(function (d) {
                    console.log(d.data)
                    if (d.data) {
                        $scope.uploading = true;
                        toastr.success("Yüklendi");
                        formdata = new FormData;
                    } else {
                        $scope.uploading = true;
                        toastr.error("Doğru dosyayı yüklediğinizden emin olunuz. Tekrar deneyiniz.");
                        formdata = new FormData;
                    }

                }, function () {

                    $scope.uploading = true;

                    toastr.error("Tekrar yükleyiniz.");
                });

        }

    }])
    constraintApp.controller('excelTreeCtrl', ['$scope', '$filter', '$location', '$http', function ($scope, $filter, $location, $http) {
        $scope.uploading = true;
        var formdata = new FormData();
        $scope.getTheFiles = function ($files) {
            angular.forEach($files, function (value, key) {
                formdata.append(key, value);
            });
        };

        // NOW UPLOAD THE FILES.
        $scope.uploadFiles = function () {
            $scope.uploading = false;
            var request = {
                method: 'POST',
                url: '/ProductTree/Upload',
                data: formdata,
                headers: {
                    'Content-Type': undefined
                }
            };


            $http(request)
                .then(function (d) {
                    console.log(d.data)
                    if (d.data) {
                        $scope.uploading = true;
                        toastr.success("Yüklendi");
                        formdata = new FormData;
                    } else {
                        $scope.uploading = true;
                        toastr.error("Doğru dosyayı yüklediğinizden emin olunuz. Tekrar deneyiniz.");
                        formdata = new FormData;
                    }

                }, function () {

                    $scope.uploading = true;

                    toastr.error("Tekrar yükleyiniz.");
                    formdata = new FormData;
                });

        }

    }])
    constraintApp.controller('homeCtrl', ['$scope', '$filter', '$location', '$http', function ($scope, $filter, $location, $http) {
        $scope.itemsUsers = [];
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.blockText = "";
        $scope.isBlock = false;
        $scope.getData = function () {

            $http({
                method: "get",
                url: "/User/GetManager"
            }).then(function (response) {
                $scope.itemsUsers = response.data;
                $scope.allExpanded = $scope.itemsUsers[0].permissionForConstraint;
            }, function () {
                toastr.error("Bir şeyler ters gitti.");
            })
        };

        $scope.sortBy = function (column) {
            $scope.sortColumn = column;
            $scope.reverse = !$scope.reverse;
        };
        $scope.Block = function () {
            if ($scope.allExpanded) {
                $scope.isBlock = false;
                $scope.updateMethod("Block", $scope.isBlock);

            } else {
                $scope.isBlock = true;
                $scope.updateMethod("UnBlock", $scope.isBlock);
            }
            $scope.allExpanded = !$scope.allExpanded;
        };
        $scope.updateMethod = function (isBlock) {
            $scope.uploading = false;
            $http.post(`/User/BlockOrUnblock?isBlock=${$scope.isBlock}`, $scope.itemsUsers).then(function (result) {
                if (result.data)
                    toastr.success("Başarılı");
                else
                    toastr.error("Başarısız");
                $scope.uploading = true;
            }, function () {
                toastr.error("Bir şeyler ters gitti.");
                $scope.uploading = true;
            })
        };
    }])
    constraintApp.controller('loginCtrl', ['$scope', '$filter', '$location', '$http', function ($scope, $filter, $location, $http) {
        $scope.userList = [];
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.username = "";
        $scope.password = "";
        $scope.type = "";
        $scope.blockControl = false;

        $scope.getData = function () {
            $http.get('/User/GetManager').then(function (result) {

                $scope.$watch('searchText', function (term) {

                    $scope.userList = $filter('filter')(result.data, term);
                    var i = 0;
                    $scope.userList = $scope.userList.map(x => { return { ...x, id: ++i } });
                    $scope.type = "";
                    $scope.password = "";
                    $scope.username = "";
                    $scope.controlIsBlock();
                });
            });
        }
        $scope.Add = function () {
            $scope.controlIsBlock();
            if ($scope.type.length === 0) {
                toastr.warning("Hesap Türü Seçiniz.");
            } else if ($scope.username.length === 0) {
                toastr.warning("Mail Girmelisiniz.");
            } else if ($scope.password.length < 3) {
                toastr.warning("Şifre En Küçük 3 Karakter içermelidir.");
            } else if (
                $scope.userList.filter((t) => t.email === $scope.username).length !== 0
            ) {
                toastr.warning($scope.username + " kullanıcı zaten kayıtlı.");

            } else {
                const today = new Date();
                if ($scope.type === "Kullanıcı") $scope.type = "U";
                else if ($scope.type === "Yönetici") $scope.type = "A";
                $http.post('/User/Create', {
                    userType: $scope.type,
                    password: $scope.password,
                    email: $scope.username,
                    isActive: true,
                    permissionForConstraint: $scope.blockControl,
                    createdTime: new Date().toISOString().substr(0, 10)
                }).then(function (result) {
                    if (result.data != null) {
                        toastr.success("Eklendi.");
                        $scope.type = "";
                        $scope.password = "";
                        $scope.username = "";
                    }
                })
            }

        };

        $scope.sortBy = function (column) {
            $scope.sortColumn = column;
            $scope.reverse = !$scope.reverse;
        };
        $scope.controlIsBlock = function () {
            $http.get(`/User/HasPermissionConstraint?hasPermissionConstraint=${false}`).then(function (result) {
                if (result.data) {
                    $scope.blockControl = false;
                } else {
                    $scope.blockControl = true;
                }
            });
        };
    }])
    constraintApp.controller('meetingCtrl', ['$scope', '$filter', '$location', '$http', function ($scope, $filter, $location, $http) {
        $scope.meetingList = [];
        $scope.itemUpdated = [];
        $scope.multipleCopyItem = [];
        $scope.copylineCount = 0;
        $scope.persons = [];
        $scope.teams = [];
        $scope.reasons = [];
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.choseAll = true;
        $scope.uploading = false;
        $scope.isUsingDateFilter = false;
        $scope.startDate = new Date().toISOString().substr(0, 10);
        $scope.endDate = new Date().toISOString().substr(0, 10);
        $scope.message = "";

        $scope.fetchData = async function () {
            $scope.meetingList = [];
            await $http.get('/Meeting/GetManager').then(function (response) {
                if (response.data != null && response.data.length != 0) {
                    var i = 0;
                    $scope.meetingList = response.data.map((x) => {
                        return { ...x, id: ++i };
                    });
                    $scope.uploading = true;
                }
                else
                    $scope.uploading = true;
            }, function () {
                $scope.uploading = true;
            })
            $scope.isUsingDateFilter = false;
        };
        $scope.fetchDataFromDate = function () {
            $scope.setDateFilter();
            $http.get('/Meeting/GetByDate?startDate=' + $scope.startDate + '&endDate=' + $scope.endDate).then(function (response) {
                if (response.data != null && response.data.length != 0) {

                    var i = 0;
                    $scope.meetingList = response.data.map((x) => {
                        return { ...x, id: ++i };
                    });
                    $scope.isUsingDateFilter = true;
                } else
                    $scope.meetingList = [];
            })

        };
        $scope.setDateFilter = function () {
            if (document.getElementById("data-endDate") != null && document.getElementById("data-startDate") != null) {
                $scope.endDate = document.getElementById("data-endDate").value;
                $scope.startDate = document.getElementById("data-startDate").value;
            }

        };

        $scope.sortBy = function (column) {
            $scope.sortColumn = column;
            $scope.reverse = !$scope.reverse;
        };

        $scope.copyLine = function (item) {
            var list = {
                constraintID: item.constraintID,
                materialCode: item.materialCode,
                materialText: item.materialText,
                productCode: item.productCode,
                plannedDate: item.plannedDate,
                amount: item.amount,
                customer: item.customer,
                version: item.version,
                delayID: item.delayID,
                delayCode: item.delayCode,
                delayAmount: item.delayAmount,
                delayDate: item.delayDate,
                delayReason: item.delayReason,
                delayDetail: item.delayDetail,
                companyTeam: item.companyTeam,
                chargePerson: item.chargePerson,
                dateCurrent: item.dateCurrent,
                changedAmount: 1,
                companyTeamCode: item.companyTeamCode,
            };
            $http.post('/Meeting/Create', list).then(function (response) {
                if ($scope.isUsingDateFilter) $scope.fetchDataFromDate();
                else $scope.fetchData();
                toastr.success("Kopyalandı");
                // $scope.message=("Kopyalandı");
                //$scope.successModal=true;
            })
        };
        $scope.deleteItem = function (item) {
            $http.post('/Meeting/Delete/' + item.constraintID).then(function (response) {
                if (response.data)
                    toastr.success("Silindi");
                if ($scope.isUsingDateFilter) $scope.fetchDataFromDate();
                else $scope.fetchData();
            })
        };
        $scope.yesItem = function () {
            if ($scope.meetingList.length !== 0) {
                $scope.uploading = false;
                $http.get('/Meeting/DeleteAll').then(function (response) {
                    if (response.data)
                        toastr.success("Hepsi Silindi");
                    $scope.fetchData();
                    $scope.uploading = true;
                }, function () {
                    toastr.error("Bir sorun oluştu.");
                    $scope.uploading = true;
                })
            }
            else
                toastr.warning("Tablo Boş")
        };
        $scope.deleteAllItem = function () {
            $scope.message = "Hepsi silinecek emin misiniz?";
        };
        $scope.toogleUpdeteButon = function (item) {
            $scope.itemUpdated = item;
        };
        $scope.updateMethod = function (item) {
            var list = {
                constraintID: item.constraintID,
                materialCode: item.materialCode,
                materialText: item.materialText,
                productCode: item.productCode,
                plannedDate: item.plannedDate,
                amount: item.amount,
                customer: item.customer,
                version: item.version,
                delayID: item.delayID,
                delayCode: item.delayCode,
                delayAmount: item.delayAmount,
                delayDate: item.delayDate,
                delayReason: item.delayReason,
                delayDetail: item.delayDetail,
                companyTeam: item.companyTeam,
                chargePerson: item.chargePerson,
                dateCurrent: item.dateCurrent,
                changedAmount: 1,
                companyTeamCode: item.companyTeamCode,
            };
            $http.post('/Meeting/Edit', list).then(function (response) {
                toastr.success("Güncelleme Başarılı");
                if ($scope.isUsingDateFilter) $scope.fetchDataFromDate();
                else $scope.fetchData();
            })
        };
        $scope.fetchReasons = function () {
            $http.get('/PostponementReason/GetManager').then(function (response) {
                $scope.reasons = response.data.map((x) => x.delayName);
            })
        };
        $scope.fetchPersons = function () {
            $http.get('/User/GetManager').then(function (response) {
                $scope.persons = response.data.map((x) => x.userName);
            })
        };
        $scope.fetchTeams = function () {

            $http.get('/CompanyTeam/GetManager').then(function (response) {

                $scope.teams = response.data.map((x) => x.companyName);
            })
        };
        $scope.multipleCopyMethod = function (item) {
            $scope.copylineCount = document.getElementById("data-copyCount").value;
            if ($scope.copylineCount > 0) {
                $scope.uploading = false;
                $http.post(`/Meeting/CreateAll?count=${$scope.copylineCount}`, item,).then(function (response) {
                    if (response.data)
                        toastr.success("Kopyalandı");
                    if ($scope.isUsingDateFilter) $scope.fetchDataFromDate();
                    else $scope.fetchData();
                    $scope.uploading = true;
                }, function () {
                    toastr.error("Bir sorun oluştu.");
                    $scope.uploading = true;
                })
            }
            else
                toastr.warning("Sıfırdan büyük bir değer giriniz.")
        };
        $scope.toogleMultipleCopy = function (item) {
            $scope.copylineCount = 0;
            $scope.multipleCopyItem = item;
        };
    }])
    constraintApp.controller('meetingTeamCtrl', ['$scope', '$filter', '$location', '$http', function ($scope, $filter, $location, $http) {
        $scope.meetingList = [];
        $scope.itemUpdated = [];
        $scope.multipleCopyItem = [];
        $scope.copylineCount = 0;
        $scope.persons = [];
        $scope.teams = [];
        $scope.reasons = [];
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.choseAll = true;
        $scope.uploading = false;
        $scope.isUsingDateFilter = false;
        $scope.startDate = new Date().toISOString().substr(0, 10);
        $scope.endDate = new Date().toISOString().substr(0, 10);
        $scope.message = "";

        $scope.fetchData = function () {
            $scope.meetingList = [];
            $http.get('/MeetingTeam/GetManager').then(function (response) {
                if (response.data != null && response.data.length != 0) {
                    var i = 0;
                    $scope.meetingList = response.data.map((x) => {
                        return { ...x, id: ++i };
                    });
                    $scope.uploading = true;
                }
                else
                    $scope.uploading = true;
            }, function () {
                $scope.uploading = true;
                toastr.error("Yüklenemedi.");
            })
        };
        $scope.fetchDataFromDate = function () {
            $scope.setDateFilter();
            $scope.uploading = false;
            $http({
                method: "get",
                url: `/MeetingTeam/GetByDate?startDate=${$scope.startDate}&endDate=${$scope.endDate}`,
            }).then(function (response) {
                if (response.data != null && response.data.length != 0) {
                    var i = 0;
                    $scope.meetingList = response.data.map((x) => {
                        return { ...x, id: ++i };
                    });
                    $scope.uploading = true;
                }
                else
                    $scope.uploading = true;

                $scope.isUsingDateFilter = true;
            }, function () {
                $scope.uploading = true;
                toastr.error("Yüklenemedi.");
            })

        };
        $scope.setDateFilter = function () {
            $scope.endDate = document.getElementById("data-endDate").value;
            $scope.startDate = document.getElementById("data-startDate").value;
        };

        $scope.sortBy = function (column) {
            $scope.sortColumn = column;
            $scope.reverse = !$scope.reverse;
        };
        $scope.deleteItem = function (item) {
            $http.post('/MeetingTeam/Delete/' + item.constraintID).then(function (response) {
                if (response.data) {
                    toastr.success("Silindi.");
                } else {
                    toastr.error("Silinemedi");
                }
                if ($scope.isUsingDateFilter) $scope.fetchDataFromDate();
                else $scope.fetchData();
            }, function () {
                $scope.uploading = true;
                toastr.error("Bir şeyler ters gitti.");
            })
        };
        $scope.deleteAll = function () {
            $http.get('/MeetingTeam/DeleteAll').then(function (response) {
                if (response.data) {
                    toastr.success("Hepsi silindi.");
                } else {
                    toastr.error("Bazı silinmeyen veriler kalmış olabilir");
                }
                if ($scope.isUsingDateFilter) $scope.fetchDataFromDate();
                else $scope.fetchData();
            }, function () {
                $scope.uploading = true;
                toastr.error("Bir şeyler ters gitti.");
            })
        };
        $scope.yesItem = function () {
            if ($scope.meetingList.length !== 0) $scope.deleteAll();
        };
        $scope.deleteAllItem = function () {
            $scope.message = "Hepsi silinecek emin misiniz?";
        };
        $scope.toogleUpdeteButon = function (item) {
            $scope.fetchPersons();
            $scope.fetchReasons();
            $scope.fetchTeams();
            $scope.itemUpdated = item;
        };
        $scope.updateMethod = function (item) {
            item.delayDate = document.getElementById("data-delay").value;
            item.dateCurrent = document.getElementById("data-current").value;
            var list = {
                constraintID: item.constraintID,
                materialCode: item.materialCode,
                materialText: item.materialText,
                productCode: item.productCode,
                plannedDate: item.plannedDate,
                amount: item.amount,
                customer: item.customer,
                version: item.version,
                delayID: item.delayID,
                delayCode: item.delayCode,
                delayAmount: item.delayAmount,
                delayDate: item.delayDate,
                delayReason: item.delayReason,
                delayDetail: item.delayDetail,
                companyTeam: item.companyTeam,
                chargePerson: item.chargePerson,
                dateCurrent: item.dateCurrent,
                changedAmount: 1,
                companyTeamCode: item.companyTeamCode,
            };
            $http.post('/MeetingTeam/Edit', list).then(function (response) {
                toastr.success("Güncelleme Başarılı");
                if ($scope.isUsingDateFilter) $scope.fetchDataFromDate();
                else $scope.fetchData();
            }, function () {
                $scope.uploading = true;
                toastr.error("Bir şeyler ters gitti.");
            })
        };
        $scope.fetchReasons = function () {
            $http.get('/PostponementReason/GetManager').then(function (response) {
                $scope.reasons = response.data.map((x) => x.delayName);
            }, function () {
                $scope.uploading = true;
                toastr.error("Bir şeyler ters gitti.");
            })

        };
        $scope.fetchPersons = function () {
            $http.get('/User/GetManager').then(function (response) {
                $scope.persons = response.data.map((x) => x.userName);
            }, function () {
                $scope.uploading = true;
                toastr.error("Bir şeyler ters gitti.");
            })

        };
        $scope.fetchTeams = function () {

            $http.get('/CompanyTeam/GetManager').then(function (response) {

                $scope.teams = response.data.map((x) => x.companyName);
            }, function () {
                $scope.uploading = true;
                toastr.error("Bir şeyler ters gitti.");
            })

        };
    }])
    constraintApp.controller('montagePlanCtrl', ['$scope', '$filter', '$location', '$http', function ($scope, $filter, $location, $http) {
        $scope.montagePlan = [];
        $scope.uploading = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;


        $scope.getData = function () {

            $http.get('/MontagePlan/GetManager').then(function (response) {

                $scope.$watch('searchText', function (term) {

                    $scope.montagePlan = $filter('filter')(response.data, term);
                    var i = 0;
                    $scope.montagePlan = $scope.montagePlan.map((x) => {
                        return { ...x, id: ++i };
                    });
                    $scope.uploading = true;
                });
            }, function () {
                alert("Error Occur");
            })
        }

        $scope.sortBy = function (column) {
            $scope.sortColumn = column;
            $scope.reverse = !$scope.reverse;
        };
    }])
    constraintApp.controller('postponementReasonCtrl', ['$scope', '$filter', '$location', '$http', function ($scope, $filter, $location, $http) {
        $scope.delayReason = [];
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.toggleCreateBtn = true;
        $scope.toggleUpdateBtn = false;
        $scope.delayName = "";


        getData();
        function getData() {
            $http.get('/PostponementReason/GetManager').then(function (result) {

                $scope.$watch('searchText', function (term) {

                    $scope.delayReason = $filter('filter')(result.data, term);
                    var i = 0;
                    $scope.delayReason = $scope.delayReason.map(x => { return { ...x, id: ++i } });
                });
            });
        }
        $scope.Delete = function (i) {
            $http.post('/PostponementReason/Delete/' + i.delayID).then(function (result) {
                if (result.data) {
                    toastr.success(i.delayName + " silindi.");
                    getData();
                }
            })
        };

        $scope.sortBy = function (column) {
            $scope.sortColumn = column;
            $scope.reverse = !$scope.reverse;
        };
        $scope.InsertData = function () {
            if ($scope.delayName !== null && $scope.delayName !== "") {
                $scope.delayReasonDTO = {};
                $scope.delayReasonDTO.delayName = $scope.delayName;
                $http.post('/PostponementReason/Create', $scope.delayReasonDTO).then(function (result) {
                    if (result.data != null) {
                        toastr.success($scope.delayName + " eklendi.");
                        $scope.toggleCreateBtn = false;
                        getData();
                        $scope.delayName = "";
                    }
                })
            }
            else toastr.warning("Sebep giriniz.");
        };
        $scope.UpdateData = function () {

            $scope.delayReasonDTO = {};
            $scope.delayReasonDTO.delayID = $scope.delayIDUpdate;
            $scope.delayReasonDTO.delayName = $scope.delayNameUpdate;
            if ($scope.delayNameUpdate !== null && $scope.delayNameUpdate !== "") {

                $http.post('/PostponementReason/Edit', $scope.delayReasonDTO).then(function (result) {
                    if (result.data != null) {
                        toastr.success($scope.delayReasonDTO.delayName + " düzenlendi.");
                        getData();
                        $scope.delayIDUpdate = "";
                        $scope.delayNameUpdate = "";
                    }
                })
            }
            else toastr.warning("Sebep giriniz.");

        };
        $scope.UpdateDataH = function (i) {
            $scope.delayIDUpdate = i.delayID;
            $scope.delayNameUpdate = i.delayName;
        };
    }])
    constraintApp.controller('productTreeCtrl', ['$scope', '$filter', '$location', '$http', function ($scope, $filter, $location, $http) {
        $scope.productTree = [];
        $scope.uploading = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;


        $scope.getData = function () {
            $http.get('/ProductTree/GetManager').then(function (response) {
                var i = 0;
                $scope.productTree = response.data.map((x) => {
                    return { ...x, id: ++i };
                });
                $scope.$watch('searchText', function (term) {
                    $scope.productTree = $filter('filter')(response.data, term);
                    var i = 0;
                    $scope.productTree = $scope.productTree.map((x) => {
                        return { ...x, id: ++i };
                    });
                    $scope.uploading = true;
                });

            }, function (e) {
                alert(e);
            })
        }

        $scope.sortBy = function (column) {
            $scope.sortColumn = column;
            $scope.reverse = !$scope.reverse;
        };
    }])
    constraintApp.controller('registerCtrl', ['$scope', '$filter', '$location', '$http', function ($scope, $filter, $location, $http) {
        $scope.userList = [];
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.username = "";
        $scope.email = "";
        $scope.password = "";
        $scope.type = "";
        $scope.blockControl = false;

        $scope.getData = function () {
            $http.get('/User/GetManager').then(function (result) {

                $scope.$watch('searchText', function (term) {

                    $scope.userList = $filter('filter')(result.data, term);
                    var i = 0;
                    $scope.userList = $scope.userList.map(x => { return { ...x, id: ++i } });
                    $scope.type = "";
                    $scope.password = "";
                    $scope.username = "";
                    $scope.email = "";
                    $scope.controlIsBlock();
                });
            });
        }
        $scope.Add = function () {
            $scope.controlIsBlock();
            if ($scope.type.length === 0) {
                toastr.warning("Hesap Türü Seçiniz.");
            } else if ($scope.username.length === 0) {
                toastr.warning("İsim Soyisim Girmelisiniz.");
            } else if ($scope.email.length === 0) {
                toastr.warning("Sicil No Girmelisiniz.");
            }
            else if ($scope.password.length < 3 || $scope.password.length > 10) {
                toastr.warning("Şifre en az3 en fazla 10 karakter içermelidir.");
            } else if (
                $scope.userList.filter((t) => t.email === $scope.email).length !== 0
            ) {
                toastr.warning($scope.email + " kullanıcı zaten kayıtlı.");

            } else {

                const today = new Date();
                if ($scope.type === "Kullanıcı") $scope.type = "U";
                else if ($scope.type === "Yönetici") $scope.type = "A";
                $http.post('/User/Create', {
                    userType: $scope.type,
                    password: $scope.password,
                    email: $scope.email,
                    userName: $scope.username,
                    isActive: true,
                    permissionForConstraint: $scope.blockControl,
                    createdTime: new Date().toISOString().substr(0, 10)
                }).then(function (result) {
                    console.log(result)
                    if (result != null) {
                        toastr.success("Eklendi.");
                        $scope.type = "";
                        $scope.password = "";
                        $scope.username = "";
                        $scope.email = "";
                    }
                })
            }

        };

        $scope.sortBy = function (column) {
            $scope.sortColumn = column;
            $scope.reverse = !$scope.reverse;
        };
        $scope.controlIsBlock = function () {
            $http.get(`/User/HasPermissionConstraint?hasPermissionConstraint=${false}`).then(function (result) {
                if (result.data) {
                    $scope.blockControl = false;
                } else {
                    $scope.blockControl = true;
                }
            });
        };
    }])
    constraintApp.controller('reportTeamCtrl', ['$scope', '$filter', '$location', '$http', function ($scope, $filter, $location, $http) {
        $scope.meetingList = [];
        $scope.meetingListReasons = [];


        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.toggleButton = true;
        $scope.uploading = false;
        $scope.isUsingDateFilter = false;
        $scope.startDate = new Date().toISOString().substr(0, 10);
        $scope.endDate = new Date().toISOString().substr(0, 10);
        $scope.message = "";

        $scope.fetchData = function () {
            $scope.meetingList = [];
            $scope.uploading = false;
            $http.get('/MeetingTeam/GetByReason').then(function (response) {
                var i = 0;
                $scope.meetingListReasons = response.data.map((x) => {
                    return { ...x, hide: true };
                });
                $scope.fetchTeamName();
                $scope.uploading = true;
            }, function () {
                $scope.uploading = true;
            })
        };
        $scope.fetchTeamName = function () {
            $scope.meetingList = [];
            $http.get('/MeetingTeam/GetByTeam').then(function (response) {
                var i = 0;
                $scope.meetingList = response.data.map((x) => {
                    return { ...x, hide: true };
                });
                $scope.uploading = true;
            }, function () {
                $scope.uploading = true;
                toastr.error("Yüklenemedi.");
            })
        };
        $scope.fetchDataFromDateTeam = function () {
            $scope.setDateFilter();
            $http.get('/MeetingTeam/GetByDateTeam?startDate=' + $scope.startDate + '&endDate=' + $scope.endDate).then(function (response) {

                var i = 0;
                $scope.meetingList = response.data.map((x) => {
                    return { ...x, hide: true };
                });
                $scope.isUsingDateFilter = true;
            }, function () {
                $scope.uploading = true;
                toastr.error("Yüklenemedi.");
            })
        };
        $scope.fetchDataFromDateReason = function () {
            $scope.setDateFilter();
            $http.get('/MeetingTeam/GetByDateReason?startDate=' + $scope.startDate + '&endDate=' + $scope.endDate).then(function (response) {

                var i = 0;
                $scope.meetingList = response.data.map((x) => {
                    return { ...x, hide: true };
                });
                $scope.fetchDataFromDateTeam();
                $scope.isUsingDateFilter = true;
            })
                .catch((e) => {
                    $scope.uploading = true;
                });
        };
        $scope.setDateFilter = function () {
            $scope.endDate = document.getElementById("data-endDate").value;
            $scope.startDate = document.getElementById("data-startDate").value;
        };

        $scope.sortBy = function (column) {
            $scope.sortColumn = column;
            $scope.reverse = !$scope.reverse;
        };
        $scope.setButtonLocation = function () {
            if ($scope.meetingList.length !== 0) {
                if ($scope.toggleButton) {
                    $scope.meetingList.map((x) => {
                        x.hide = true
                    });

                } else {
                    $scope.meetingList.map((x) => {
                        x.hide = false
                    });
                }
                $scope.toggleButton = !$scope.toggleButton;
            }
        };

        $scope.Export = function () {
            if ($scope.meetingList.length !== 0 || $scope.isUsingDateFilter === true) {
                $scope.uploading = false;
                $http.get('/MeetingTeam/Export', { responseType: 'blob' }).then(function (response) {
                    $scope.uploading = true;
                    var date = new Date().toISOString().substr(0, 10);
                    //var myBlob = new Blob([result.data], {type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'})
                    const url = window.URL.createObjectURL(response.data);
                    const link = document.createElement("a");
                    link.href = url;
                    link.setAttribute("download", `TAKIMLAR//${date}.xlsx`); //or any other extension
                    document.body.appendChild(link);
                    link.click();
                    toastr.success("Excel Dosyasına Aktarıldı.");
                });
            } else {
                toastr.error("Tablo Boş");
            }
        };
        /**
    * Iki tarih arasındaki exportlama methodur.
    */
        $scope.ExportByDate = function () {
            if ($scope.meetingList.length !== 0) {
                $scope.uploading = false;
                $http.get(`/MeetingTeam/ExportByDate?startDate=${$scope.startDate}&endDate=${$scope.endDate}`, { responseType: 'blob' }).then(function (response) {
                    $scope.uploading = true;
                    var date = new Date().toISOString().substr(0, 10);
                    //var myBlob = new Blob([result.data], {type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'})
                    const url = window.URL.createObjectURL(response.data);
                    const link = document.createElement("a");
                    link.href = url;
                    link.setAttribute("download", `TAKIMLAR//${date}.xlsx`); //or any other extension
                    document.body.appendChild(link);
                    link.click();
                    toastr.success("Excel Dosyasına Aktarıldı.");
                });
            } else {
                toastr.error("Tablo Boş");
            }
        };

    }])
    constraintApp.controller('userListCtrl', ['$scope', '$filter', '$location', '$http', function ($scope, $filter, $location, $http) {
        $scope.userList = [];
        $scope.userType = "";
        $scope.userID = "";
        $scope.userName = "";
        $scope.email = "";
        $scope.password = "";
        $scope.permissionForConstraint = "";
        $scope.isActive = "";
        $scope.uploading = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.getData = function () {
            $http.get('/User/GetManager').then(function (response) {
                $scope.$watch('searchText', function (term) {
                    $scope.userList = $filter('filter')(response.data, term);
                    $scope.userList = $scope.userList.map((t) => $scope.changeType(t));
                    var i = 0;
                    $scope.userList = $scope.userList.map(x => { return { ...x, id: ++i } });
                    $scope.uploading = true;
                });
            }, function () {
                toastr.error("Bir şeyler ters gitti.");
            })
        };

        $scope.sortBy = function (column) {
            $scope.sortColumn = column;
            $scope.reverse = !$scope.reverse;
        };
        $scope.changeType = function (userList) {
            if (userList.userType === "A") userList.userType = "Yönetici";
            else if (userList.userType === "U") userList.userType = "Kullanıcı";
            return userList;
        };
        $scope.deleteMethod = function (item) {
            $http.post('/User/Control/' + item.userID).then(function (result) {
                if (result.data)
                    toastr.error("Kendi hesabınızı silemezsiniz");
                else {
                    $http.post('/User/Delete/' + item.userID).then(function (result) {
                        if (result.data) {
                            toastr.success(item.email + " silindi.");
                            $scope.getData();
                        }
                    }, function () {
                        toastr.error("Bir şeyler ters gitti.");
                    })
                }
            }, function () {
                toastr.error("Tekrar Deneyiniz.");
            })

        };
        $scope.updateMethod = function () {
            if ($scope.userType === "Yönetici") $scope.userType = "A";
            else if ("Kullanıcı") $scope.userType = "U";
            var item = {
                userID: $scope.userID,
                userType: $scope.userType,
                userName: document.getElementById('userName').value,
                email: document.getElementById('email').value,
                password: document.getElementById('password').value,
                permissionForConstraint: $scope.permissionForConstraint,
                isActive: $scope.isActive
            };
            if (item.password.length < 11 && item.password.length > 2) {

                $http.post('/User/Edit', item).then(function (result) {
                    if (result.data != null) {
                        $scope.getData();
                        toastr.success("Güncellendi.");
                    }
                }, function () {
                    toastr.error("Bir şeyler ters gitti.");
                })
            } else {
                toastr.warning("Şifre en az 3 en fazla 10 karakterli olmalıdır.")
            }
        };
        $scope.UpdateData = function (i) {
            $scope.userType = i.userType;
            $scope.userID = i.userID;
            $scope.userName = i.userName;
            $scope.email = i.email;
            $scope.password = i.password;
            $scope.permissionForConstraint = i.permissionForConstraint;
            $scope.isActive = i.isActive;
            $scope.controlUserToken(i);

        };
        $scope.controlUserToken = function (i) {
            $http.post('/User/Control/' + i.userID).then(function (result) {
                $scope.control = result.data;
            })

        };

    }])
    constraintApp.controller('versionCtrl', ['$scope', '$filter', '$location', '$http', function ($scope, $filter, $location, $http) {
        $scope.version = [];
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;


        $scope.getData = function () {
            $http.get('/Version/GetManager').then(function (response) {
                $scope.$watch('searchText', function (term) {
                    $scope.version = $filter('filter')(response.data, term);
                    var i = 0;
                    $scope.version = $scope.version.map(x => { return { ...x, id: ++i } });
                });

            }, function () {
                alert("Error Occur");
            })
        };

        $scope.Delete = function (i) {
            $http.post('/Version/Delete/' + i.versionID).then(function (result) {
                if (result.data) {
                    toastr.success(i.versionName + " silindi.");
                    $scope.getData();
                }
            })
        };

        $scope.sortBy = function (column) {
            $scope.sortColumn = column;
            $scope.reverse = !$scope.reverse;
        };
        $scope.InsertData = function () {
            if (($scope.versionName != null && $scope.versionValue != null && $scope.versionName != "" && $scope.versionValue != "")) {
                $scope.versionDTO = {};
                $scope.versionDTO.versionName = $scope.versionName;
                $scope.versionDTO.versionValue = $scope.versionValue;
                $http.post('/Version/Create', $scope.versionDTO).then(function (result) {
                    if (result.data != null) {
                        toastr.success($scope.versionName + " eklendi.");
                        $scope.versionName = "";
                        $scope.versionValue = "";
                        $scope.getData();
                    }
                })
            }
            else toastr.warning("Hat bilgisi ve değerini doğru girdiğinizden emin olunuz!");
        };
        $scope.UpdateData = function () {
            $scope.versionDTO = {};
            $scope.versionDTO.versionID = $scope.versionIDUpdate;
            $scope.versionDTO.versionName = $scope.versionNameUpdate;
            $scope.versionDTO.versionValue = $scope.versionValueUpdate;
            if ($scope.versionNameUpdate != null && $scope.versionNameUpdate != "" && $scope.versionValueUpdate != null && $scope.versionValueUpdate != "") {
                $http.post('/Version/Edit', $scope.versionDTO).then(function (result) {
                    if (result.data != null) {
                        toastr.success($scope.versionDTO.versionName + " düzenlendi.");
                        $scope.versionIDUpdate = "";
                        $scope.versionNameUpdate = "";
                        $scope.versionValueUpdate = "";
                        $scope.getData();
                    }
                })
            }
            else toastr.warning("Hat bilgisi ve değerini doğru girdiğinizden emin olunuz!");

        };
        $scope.UpdateDataH = function (i) {
            $scope.versionIDUpdate = i.versionID;
            $scope.versionNameUpdate = i.versionName;
            $scope.versionValueUpdate = i.versionValue;
        };
    }])
    constraintApp.controller('ZKP3Ctrl', ['$scope', '$filter', '$location', '$http', function ($scope, $filter, $location, $http) {
        $scope.meetingList = [];
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.isUsingDateFilter = false;
        $scope.uploading = false;
        $scope.startDate = new Date().toISOString().substr(0, 10);
        $scope.endDate = new Date().toISOString().substr(0, 10);
        $scope.message = "";

        $scope.fetchData = function () {
            $scope.meetingList = [];
            $http.get('/Meeting/GetManager').then(function (response) {
                if (response.data != null && response.data.length != 0) {
                    var i = 0;
                    $scope.meetingList = response.data.map((x) => {
                        return { ...x, id: ++i };
                    });

                } else
                    $scope.meetingList = [];
                $scope.uploading = true;
            }, function () {
                $scope.uploading = true;
                toastr.error("Yüklenemedi.");
            })
        };
        $scope.fetchDataFromDate = function () {
            $scope.setDateFilter();
            $http.get('/Meeting/GetByDate?startDate=' + $scope.startDate + '&endDate=' + $scope.endDate).then(function (response) {
                if (response.data != null && response.data.length != 0) {
                    var i = 0;
                    $scope.meetingList = response.data.map((x) => {
                        return { ...x, id: ++i };
                    });
                }
                else
                    $scope.meetingList = [];
                $scope.isUsingDateFilter = true;
            }, function () {
                $scope.uploading = true;
                toastr.error("Yüklenemedi.");
            })
        };
        $scope.setDateFilter = function () {
            $scope.endDate = document.getElementById("data-endDate").value;
            $scope.startDate = document.getElementById("data-startDate").value;
        };

        $scope.sortBy = function (column) {
            $scope.sortColumn = column;
            $scope.reverse = !$scope.reverse;
        };

        $scope.Export = function () {
            if ($scope.meetingList.length !== 0 || $scope.isUsingDateFilter === true) {
                $scope.uploading = false;
                $http.get('/Meeting/Export', { responseType: 'blob' }).then(function (response) {
                    $scope.uploading = true;
                    var date = new Date().toISOString().substr(0, 10);
                    //var myBlob = new Blob([result.data], {type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'})
                    const url = window.URL.createObjectURL(response.data);
                    const link = document.createElement("a");
                    link.href = url;
                    link.setAttribute("download", `ZKP3_DEGISIKLIK//${date}.xlsx`);
                    document.body.appendChild(link);
                    link.click();
                    toastr.success("Excel Dosyasına Aktarıldı.");
                });
            } else {
                toastr.error("Tablo Boş");
            }
        };

        $scope.ExportByDate = function () {
            if ($scope.meetingList.length !== 0) {
                $scope.uploading = false;
                $http.get(`/Meeting/ExportByDate?startDate=${$scope.startDate}&endDate=${$scope.endDate}`, { responseType: 'blob' }).then(function (response) {
                    $scope.uploading = true;
                    var date = new Date().toISOString().substr(0, 10);
                    //var myBlob = new Blob([result.data], {type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'})
                    const url = window.URL.createObjectURL(response.data);
                    const link = document.createElement("a");
                    link.href = url;
                    link.setAttribute(
                        "download",
                        `ZKP3_DEGISIKLIK//${$scope.startDate}-${$scope.endDate}.xlsx`
                    );
                    document.body.appendChild(link);
                    link.click();
                    toastr.success("Excel Dosyasına Aktarıldı.");
                });
            } else {
                toastr.error("Tablo Boş");
            }
        };

    }])
    constraintApp.controller('profileCtrl', ['$scope', '$filter', '$location', '$http', function ($scope, $filter, $location, $http) {
        $scope.user = {};
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.password = "";

        $scope.getData = function () {
            $http.get('/HomeUser/GetProfile').then(function (result) {
                if (result.data != null && result.data.length != 0) {
                    $scope.user = result.data.Data;
                    if ($scope.user.userType === "A") $scope.user.userType = "Yönetici";
                    else if ($scope.user.userType === "U") $scope.user.userType = "Kullanıcı";
                }
                else
                    $scope.user = {};
            });
        }
        $scope.update = function () {
            if ($scope.user.password.length > 2 && $scope.user.password.length < 11) {
                if ($scope.user.userType === "Yönetici") $scope.user.userType = "A";
                else if ($scope.user.userType === "Kullanıcı") $scope.user.userType = "U";
                $http.post(`/User/Edit`, $scope.user).then(function (result) {
                    if (result.data != null || result.data.length != 0) {
                        toastr.success("Güncellendi.")
                        $scope.getData();
                    } else {
                        toastr.error("Başarısız.")
                    }
                });
            } else {
                toastr.warning("Şifreniz en az 3 en fazla 10 karakter olabilir.");
            }
        };
    }])
    constraintApp.controller('constraintsCtrl', ['$scope', '$filter', '$location', '$http', function ($scope, $filter, $location, $http) {
        $scope.constraintList = [];
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.uploading = true;


        $scope.fetchData = function () {
            $scope.constraintList = [];
            $scope.uploading = false;
            $http.get('/ConstraintEntry/GetDelayEntered').then(function (response) {
                var i = 0;
                $scope.constraintList = response.data.map((x) => {
                    return { ...x, id: ++i };
                });
                $scope.uploading = true;
            }, function () {
                $scope.uploading = true;
                toastr.error("Yüklenemedi.");
            })
        };

        $scope.sortBy = function (column) {
            $scope.sortColumn = column;
            $scope.reverse = !$scope.reverse;
        };

    }])
    constraintApp.directive('stringToNumber', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                ngModel.$parsers.push(function (value) {
                    return '' + value;
                });
                ngModel.$formatters.push(function (value) {
                    return parseFloat(value);
                });
            }
        };
    });
    constraintApp.directive('ngFiles', ['$parse', function ($parse) {

        function fn_link(scope, element, attrs) {
            var onChange = $parse(attrs.ngFiles);
            element.on('change', function (event) {
                onChange(scope, { $files: event.target.files });
            });
        };

        return {
            link: fn_link
        }
    }]);
})(window.angular);

