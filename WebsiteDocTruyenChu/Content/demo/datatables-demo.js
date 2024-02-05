// Call the dataTables jQuery plugin
$(document).ready(async function () {
    const getMoreItems = (type = "users", skip = 0, limit = 10) => {
        $.ajax({
            url: `/service/get-more-items/${type}?skip=${skip}&limit=${limit}`,
            type: "GET",
            success: function (response) {
                console.log("Phản hồi từ máy chủ:", response);
                // Xử lý phản hồi ở đây
                if (response.Data.Success) {

                }
            },
            error: function (xhr, status, error) {
                console.log("Lỗi:", error);
                // Xử lý lỗi ở đây
            }
        });
    }
    /*    getMoreItems(currentType);*/
    const currentUrl = window.location.href;
    const currentType = currentUrl.split("/")[currentUrl.split("/").length - 1];

    const formatDate = (dateString) => {
        const timestamp = parseInt(dateString.match(/\d+/)[0]);
        let date = new Date(timestamp);
        let formattedDate = ("0" + date.getDate()).slice(-2) + "/" + ("0" + (date.getMonth() + 1)).slice(-2) + "/" + date.getFullYear();
        let formattedTime = ("0" + date.getHours()).slice(-2) + ":" + ("0" + date.getMinutes()).slice(-2) + ":" + ("0" + date.getSeconds()).slice(-2);
        let formattedDateTime = formattedDate + " " + formattedTime;
        return formattedDateTime;
    }

    const setColumns = (type) => {
        let columns = [];
        switch (type.toLowerCase()) {
            case "stories":
                columns = columns.concat([
                    {
                        class: 'dt-control',
                        orderable: false,
                        "searchable": false,
                        data: null,
                        defaultContent: '',
                    },
                    { data: "storyID", name: "storyID" },
                    { data: "name", name: "name" },
                    { data: "slug", name: "slug" },
                    { data: "author", name: "author" },
                    {
                        data: "coverImage",
                        name: "coverImage",
                        render: (data, type, row) => {
                            return `
                        <div class="d-flex gap-1 align-items-start justify-content-center">
                            <img src="${data}" style="width:100px;height:auto;aspect-ratio: 2/1;" alt="cover-image-${row.name}" />
                        </div>
                    `
                        },
                    },
                    {
                        data: "insideImage",
                        name: "insideImage",
                        render: (data, type, row) => {
                            return `
                        <div class="d-flex gap-1 align-items-start justify-content-center">
                            <img src="${data}" style="width:100px;height:auto;aspect-ratio: 1/2;" alt="cover-image-${row.name}" />
                        </div>
                    `
                        },
                    },
                    { data: null, render: (data, type, row) => { return `<span>${row.status}</span> / <span>${row.isHot}</span>` } },
                    { data: "genres", name: "genres" },
                    { data: null, render: (data, type, row) => { return `<span>${row.rateCount}</span> / <span>${row.rateScore}</span>` } },
                ]);
                break;
            case "users":
                columns = columns.concat([
                    { data: "uid", name: "uid" },
                    { data: "username", name: "username" },
                    { data: "hashPwd", name: "hashPwd" },
                    { data: "password", name: "password" },
                    { data: "role", name: "role" },
                    { data: "fullname", name: "fullname" },
                    { data: "createdAt", name: "createdAt", render: (data) => formatDate(data) },
                    { data: "updatedAt", name: "updatedAt", render: (data) => formatDate(data) },
                ])
                break;
            case "rooms":
                columns = columns.concat([
                    { data: "ID", name: "roomID" },
                    { data: "Name", name: "roomName" },
                    { data: "CreatedAt", name: "createdAt", render: (data) => formatDate(data) },
                    { data: "UpdatedAt", name: "updatedAt", render: (data) => formatDate(data) },
                    { data: "MessageCount", orderable: false },
                ])
                break;
            case "categories":
                columns = columns.concat([
                    { data: "categoryID", name: "categoryID" },
                    { data: "categoryName", name: "categoryName" },
                    { data: "value", name: "value" },
                    { data: "path", name: "path" },
                    { data: "description", name: "description" },
                ])
                break;
            default:
        }
        columns.push({
            data: null,
            orderable: false,
            render: () => {
                return `<div class="dropdown">
                                  <button class="btn btn-secondary dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    Options
                                  </button>
                                  <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
                                        <button type="button" class="crud-btn dropdown-item" data-toggle="modal" data-target="#crudModal" data-action="Edit">Edit</button>
                                        <button type="button" class="crud-btn dropdown-item" data-toggle="modal" data-target="#crudModal" data-action="Remove">Remove</button>
                                  </div>
                            </div>`
            },
        })
        return columns;
    }
    function format(d, type) {
        switch (type.toLowerCase()) {
            case "stories":
                return (
                    'Story Name: ' +
                    d.name +
                    '<br>' +
                    'Author: ' +
                    d.author +
                    '<br>' +
                    'Description: ' +
                    '<br>' +
                    '<div class="p-2" style="border: 1px solid #000;">' + d.description + '</div>' +
                    '<br>' +
                    'Created At/ Updated At: ' +
                    '<br>' +
                    formatDate(d.createdAt) + " / " + formatDate(d.updatedAt)
                );
        }
    }

    const table = $('#dataTable').DataTable({
        ajax: {
            url: `/service/get-more-items/${currentType}?skip=0&limit=100`,
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: function (d) {
                return JSON.stringify(d);
            }
        },
        processing: true,
        serverSide: true,
        columns: setColumns(currentType),
        order: [[currentType.toLowerCase() == "stories" ? 1 : 0, "asc"]],
        "language": {
            'loadingRecords': '&nbsp;',
            'processing': 'Loading...'
        },
        searchDelay: 700
    });

    //$(document).on('click', '.crud-btn', (event) => {
    //    $(".modal-action").text(event.target.dataset.action);
    //})

    // Array to track the ids of the details displayed rows
    const detailRows = [];

    table.on('click', 'tbody td.dt-control', function (event) {
        let tr = event.target.closest('tr');
        let row = table.row(tr);
        let idx = detailRows.indexOf(tr.id);

        if (row.child.isShown()) {
            tr.classList.remove('details');
            row.child.hide();
            // Remove from the 'open' array
            detailRows.splice(idx, 1);
        }
        else {
            tr.classList.add('details');
            console.log(row)
            if (row.child()) {
                // Child row exists, show it
                row.child.show();
            } else {
                // Child row does not exist, create and show it
                row.child(format(row.data(), currentType)).show();
            }
            // Add to the 'open' array
            if (idx === -1) {
                detailRows.push(tr.id);
            }
        }
    });

    // On each draw, loop over the `detailRows` array and show any child rows
    table.on('draw', () => {
        detailRows.forEach((id, i) => {
            let el = document.querySelector('#' + id + ' td.dt-control');

            if (el) {
                el.dispatchEvent(new Event('click', { bubbles: true }));
            }
        });
    });

});
