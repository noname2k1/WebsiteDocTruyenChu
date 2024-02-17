import { removeDiacritics } from '../helpers.js'
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
    const currentType = currentUrl.split("/")[currentUrl.split("/").length - 1].split("?")[0];
    const query = currentUrl.split("/")[currentUrl.split("/").length - 1].split("?")[1]?.split("=")[1] ?? "";

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
                //console.log(query)
                if (query) {
                    columns = columns.concat([
                        {
                            class: 'dt-control',
                            orderable: false,
                            "searchable": false,
                            data: null,
                            defaultContent: '<a href="#">Expand</a>',
                        },
                        { data: "storyChapterID", name: "storyChapterID" },
                        { data: "title", name: "title" },
                        { data: "slug", name: "slug" },
                        { data: "content", name: "content", class: "d-none" },
                        { data: "views", name: "views" },
                        { data: "createdAt", name: "createdAt", render: (data) => formatDate(data) },
                        { data: "updatedAt", name: "updatedAt", render: (data) => formatDate(data) },
                    ])
                } else {
                    columns = columns.concat([
                        {
                            class: 'dt-control',
                            orderable: false,
                            "searchable": false,
                            data: null,
                            defaultContent: '<a href="#">Expand</a>',
                        },
                        { data: "storyID", name: "storyID", class: "storyID", render: (data, type, row) => `<a href="/admin/manager/stories?query=${row.slug}">${data}</a>` },
                        { data: "name", name: "name", class: "name", class: "name", render: (data, type, row) => `<a href="/admin/manager/stories?query=${row.slug}">${data}</a>` },
                        { data: "slug", name: "slug", class: "slug" },
                        { data: "author", name: "author", class: "author" },
                        {
                            data: "coverImage",
                            name: "coverImage",
                            "data-name": "coverImage",
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
                        { data: "genres", name: "genres", class: "genres" },
                        { data: null, render: (data, type, row) => { return `<span>${row.rateCount}</span> / <span>${row.rateScore}</span>` } },
                        { data: "description", name: "description", class: "description d-none" },
                    ]);
                }
                break;
            case "users":
                columns = columns.concat([
                    { data: "uid", name: "uid", class: "uid", render: (data, type, row) => `<a href="/admin/manager/user-detail?query=${row.username}">${data}</a>` },
                    { data: "username", name: "username", class: "username", render: (data, type, row) => `<a href="/admin/manager/user-detail?query=${row.username}">${data}</a>` },
                    { data: "hashPwd", name: "hashPwd", class: "hashPwd" },
                    { data: "password", name: "password", class: "password" },
                    { data: "role", name: "role", class: "role", render: (data, type, row) => data == 0 ? "admin" : data == 1 ? "moderator" : "user" },
                    { data: "fullname", name: "fullname", class: "fullname" },
                    { data: "createdAt", name: "createdAt", render: (data) => formatDate(data) },
                    { data: "updatedAt", name: "updatedAt", render: (data) => formatDate(data) },
                ])
                break;
            case "user-detail":
                columns = columns.concat([
                    {
                        data: "udID", name: "udID"
                    },
                    { data: "username", name: "username" },
                    { data: "favourites", name: "favourites" },
                    { data: "followers", name: "followers" },
                    {
                        data: "followings", name: "followings"
                    },
                    { data: "friends", name: "friends" }
                    ,
                    { data: "avatar", name: "avatar" }
                    ,
                    { data: "bio", name: "bio" }
                ])
                break;
            case "rooms":
                columns = columns.concat(query ? [
                    { data: "MESSAGEID", name: "MESSAGEID" },
                    { data: "userid", name: "userid" },
                    { data: "content", name: "content" },
                    { data: "createdAt", name: "createdAt", render: (data) => formatDate(data) },
                    { data: "updatedAt", name: "updatedAt", render: (data) => formatDate(data) },
                ] :
                    [
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
                                <button type="button" class="crud-btn dropdown-item bg-danger text-white" data-toggle="modal" data-target="#removeModal" data-action="Remove">Remove</button>
                            </div>
                        </div>`
            },
        })
        return columns;
    }
    function format(d, type) {
        switch (type.toLowerCase()) {
            case "stories":
                if (query) {
                    return (
                        'Story Name: ' +
                        d.storyName +
                        '<br>' +
                        'Content: ' +
                        '<br>' +
                        '<div class="p-2" style="border: 1px solid #000;">' + d.content + '</div>' +
                        '<br>' +
                        'Created At/ Updated At: ' +
                        '<br>' +
                        formatDate(d.createdAt) + " / " + formatDate(d.updatedAt)
                    );
                }
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
            url: `/service/get-more-items/${currentType}?skip=0&limit=100&query=${query}`,
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
        searchDelay: 700,
        "createdRow": function (row, data, dataIndex) {
            //console.log({ row, data, dataIndex })
            for (const [key, value] of Object.entries(data)) {
                $(row).attr("data-" + key.toLowerCase(), value)
            }
        },
    });

    // Array to track the ids of the details displayed rows
    const detailRows = [];

    table.on('click', 'tbody td.dt-control', function (event) {
        let tr = event.target.closest('tr');
        let row = table.row(tr);
        let idx = detailRows.indexOf(tr.id);
        const text = row.selector.rows.querySelectorAll("td")[0].querySelector("a");
        text.textContent == "Shrink" ? text.textContent = "Expand" : text.textContent = "Shrink";
        if (row.child.isShown()) {
            tr.classList.remove('details');
            row.child.hide();
            // Remove from the 'open' array
            detailRows.splice(idx, 1);
        }
        else {
            tr.classList.add('details');
            //console.log(row)
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

    // Crud parts
    let formAction = "add";

    // Add/Edit/Remove btn is Clicked
    $(document).on('click', '.crud-btn', (event) => {
        const action = event.target.dataset.action.toLowerCase();
        $(".modal-action").text(action);
        renderFormItems(action);
        formAction = action;
        const inputs = Array.from($("form.crud-form input, form.crud-form select, form.crud-form textarea"));
        if (action == "edit") {
            //console.log(inputs)
            //console.log(event.target.closest("tr"))
            inputs.forEach(element => element.value = event.target.closest("tr").dataset[element.id.toLowerCase()])
        }
        if (action == "remove") {
            $("input[type='hidden'][name='id']").val(event.target.closest("tr").querySelectorAll("td")[0].textContent);
        }
        setTimeout(() => $("form.crud-form input:not([type='hidden'])")[0].focus(), 500);
    })

    // trigger when OK btn of remove modal is clicked
    $("#remove-ok-btn").click(function () {
        const id = $(this).closest(".modal-content").find("input[type='hidden'][name='id']").val()
        if (id) {
            $(this).text("Processing...")
            fetch(`/admin/remove/${currentType}?id=${id}`, {
                method: "POST"
            }).then(raw => raw.json()).then(res => {
                if (res.Data.Success) {
                    table.clear().draw()
                    $("#removeModal").modal("hide")
                }
                alert(res.Data.message)
                $(this).text("OK")
            }).catch(err => $(this).text("OK"))
        }
    })

    const crud_fields = {
        "users": [
            { data: "uid", name: "uid", type: "hidden" },
            { data: "username", name: "username" },
            { data: "password", name: "password", autoHash: "hashPwd" },
            { data: "hashPwd", name: "hashPwd" },
            {
                data: "role", name: "role", html:
                    `<select name="role" class="form-select form-select-sm" aria-label=".form-select-sm role-select" id="role">
                          <option selected>Open role menu</option>
                          <option value="1">Moderator</option>
                          <option value="2">User</option>
                    </select>`
            },
            { data: "fullname", name: "fullname" }
        ],
        "user-detail": [
            { data: "udID", name: "udID", type: "hidden" },
            { data: "username", name: "username" },
            { data: "favourites", name: "favourites" },
            { data: "followers", name: "followers" },
            {
                data: "followings", name: "followings"
            },
            { data: "friends", name: "friends" }
            ,
            { data: "avatar", name: "avatar" }
            ,
            { data: "bio", name: "bio" }
        ],
        "categories": [
            { data: "categoryID", name: "categoryID", type: "hidden" },
            { data: "categoryName", name: "categoryName", autoSlug: "path" },
            { data: "value", name: "value" },
            { data: "path", name: "path" },
            { data: "description", name: "description" },
        ],
        "rooms": [
            { data: "ID", name: "roomID", type: "hidden" },
            { data: "Name", name: "roomName" },
        ],
        "stories": [
            { data: "storyID", name: "storyID", type: "hidden" },
            { data: "name", name: "name", autoSlug: "slug" },
            { data: "slug", name: "slug" },
            { data: "author", name: "author", ajax: "/service/authors", fields: { label: "name", value: "slug" } },
            { data: "coverImage", name: "coverImage" },
            { data: "insideImage", name: "insideImage" },
            { data: "isHot", name: "isHot" },
            { data: "genres", name: "genres", ajax: "/service/getcategories", fields: { label: "categoryName", value: "path" }, multiChoice: true },
            { data: "rateCount", name: "rateCount" },
            { data: "rateScore", name: "rateScore" },
            {
                data: "description", name: "description", html:
                    `
                    <textarea class="form-control" id="description" name="description" placeholder="Leave a Description here"></textarea>
                    `
            }
        ],
        "chapters": [],
        "messages": [],
        "user-data": []
    }

    const generatorSlug = (inputString) => {
        return removeDiacritics(inputString.trim().toLowerCase()).replaceAll(/[!@#$%^&*()_+.,]/g, "").replaceAll(' ', '-').replaceAll('--', '-');
    }

    const renderFormItems = (formAction = 'add') => {
        if (currentType) {
            $("form.crud-form").html("");
            crud_fields[currentType].forEach(field => {
                /*             console.log({ field, bool: !field.exclude || (field.exclude.toLowerCase() != formAction.toLowerCase()) })*/
                if (field.ajax && field.fields) {
                    fetch(field.ajax).then(raw => raw.json()).then(res => {
                        console.log(res);
                        if (res.Data.Success) {
                            $("form.crud-form").append(`
                                <div class="form-group mt-1">
                                    ${field.multiChoice ? `<div class="d-flex flex-wrap gap-1 ${field.name}-choice-container"></div>` : ''}
                                    <label class="col-form-label text-capitalize" for="${field.name}-choice">${field.name}</label>
                                    <input class="form-control" list="${field.name}" data-multichoice="${field.multiChoice ? true : false}" id="${field.name}-choice" name="${field.name}" readonly>
                                    <datalist id="${field.name}">
                                    ${res.Data.data.reduce((prev, current) => {
                                return prev + `<option value="${current[field.fields.label]}" data-value="${current[field.fields.value]}">`
                            }, "")}
                                    </datalist>
                                </div>
                            `)
                        }
                    }).catch(err => console.log(err))
                }
                else if (field.html) {
                    $("form.crud-form").append(`${field.type == 'hidden' ? '' : `<label for="${field.name}" class="col-form-label text-capitalize">${field.name}:</label>`}${field.html}`);
                } else {
                    $("form.crud-form").append(`
                        <div class="form-group">
                            ${field.type == 'hidden' ? '' : `<label for="${field.name}" class="col-form-label text-capitalize">${field.name}:</label>`}
                            <input type="${field.type ? field.type : 'text'}" class="form-control" data-auto-slug="${field.autoSlug ? field.autoSlug : ''}"
                            data-auto-hash="${field.autoHash ? field.autoHash : ''}" id="${field.name}" name="${field.name}">
                         </div>
                    `);
                }
            })

            $('form.crud-form input').on('input', function () {
                const autoSlugField = $(this).data("auto-slug");
                const autoHash = $(this).data("auto-hash");
                if (autoSlugField) {
                    $(`input[name="${autoSlugField}"]`).val(generatorSlug($(this).val()))
                }
                if (autoHash) {
                    $(`input[name="${autoHash}"]`).val(CryptoJS.MD5($(this).val()))
                }
            })
            tinymce.remove("#description");
            tinymce.init({
                selector: 'textarea',
                target: document.getElementById('description'),
                plugins: [
                    'a11ychecker', 'advlist', 'advcode', 'advtable', 'autolink', 'checklist', 'export',
                    'lists', 'link', 'image', 'charmap', 'preview', 'anchor', 'searchreplace', 'visualblocks',
                    'powerpaste', 'fullscreen', 'formatpainter', 'insertdatetime', 'media', 'table', 'help', 'wordcount'
                ],
                toolbar: 'undo redo | a11ycheck casechange blocks | bold italic backcolor | alignleft aligncenter alignright alignjustify |' +
                    'bullist numlist checklist outdent indent | removeformat | code table help'
            })
        }
    }

    // set data-value of datalist-option to input
    $(document).on("focus", "input[id$='-choice']", function () { $(this).val(""); $(this).prop("readonly", false); })
    $(document).on("blur", "input[id$='-choice']", function () {
        $(this).prop("readonly", true);
        if ($(this).data("multichoice")) {
            const values = [];
            Array.from($(`.${$(this).prop("name")}-choice-item`)).forEach(element => values.push(element.textContent))
            $(this).val(values.length > 0 ? JSON.stringify(values) : "")
        }
    })
    $(document).on("change", "input[id$='-choice']", function () {
        if ($(this).val()) {
            const value = $(this)[0].nextElementSibling.querySelector(`option[value="${$(this).val()}"]`).dataset["value"];
            if ($(this).data("multichoice")) {
                if (Array.from($(`.${$(this).prop("name")}-choice-item`)).every(element => element.textContent != value)) {
                    $(`.${$(this).prop("name")}-choice-container`).append(`<div class='btn btn-primary ${$(this).prop("name")}-choice-item'>${value}</div>`)
                } else {
                    Array.from($(`.${$(this).prop("name")}-choice-item`)).find(element => element.textContent == value).remove();
                }
            } else {
                $(this).val(value);
            }
            $(this).blur()
        }
    })

    $(document).on("click", "[class$='-choice-item']", function () {
        const inputName = $(this).prop("class").split(" ")[2].split("-")[0];
        let jsonValueToArray = JSON.parse($(`input[name='${inputName}']`).val())
        jsonValueToArray = jsonValueToArray.filter(value => value != $(this).text())
        $(`input[name='${inputName}']`).val(jsonValueToArray.length > 0 ? JSON.stringify(jsonValueToArray) : "")
        $(this).remove()
    })

    // excecute btn is clicked
    $('#submit-btn').click(function () {
        const allInputs = $("form.crud-form input[name], form.crud-form select[name], form.crud-form textarea[name]");
        const formData = {};
        Array.from(allInputs).forEach(input => {
            if (input.value) {
                formData[input.getAttribute("name")] = input.value;
            }
        })
        //console.log(formData)
        if (Object.keys(formData).length >= Array.from(allInputs).length - 1) {
            $(this).text("Processing...")
            fetch(`/admin/${formAction}/${currentType}`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(formData)
            }).then(raw => raw.json()).then(res => {
                if (res.Data.Success) {
                    table.clear().draw()
                    $("#crudModal").modal("hide")
                }
                alert(res.Data.message)
                $(this).text("Execute")
            }).catch(err => $(this).text("Execute"))
        } else {
            alert("Missing information");
            $("form.crud-form input:not([type='hidden'])")[0].focus()
        }
        //for (const entry of formData.entries()) {
        //    console.log(entry[0] + ": " + entry[1]);
        //}
    })
});
