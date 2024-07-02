const baseURL = "https://localhost:7147/PropertyManager/"
//const baseURL = "https://property-manager.projects.bbdgrad.com/PropertyManager/"



const customFetch = (async (url, options = {}) => {

    const defaultHeaders = {
        "Authorization": "Bearer " + localStorage.getItem("Token")
    }
    const headers = {
        ...defaultHeaders,
        ...options.headers
    }

    const mergedOptions = {
        ...options,
        headers
    }
    let response;
    try {

        response = await fetch(url, mergedOptions);
    } catch (e) {
        return (e.message)
    }

    if (response.status === 401) {
        // localStorage.removeItem("Token")
        return ("logged out")
    } else {
        try {

            const data = await response.json();
            return (data)
        } catch (e) {
            return (e.message)
        }
    }
})
const getProperties = (async (pageNumber, pageSize = 7, id = undefined, ownerId = undefined, capacity = undefined) => {

    let url = baseURL + "Properties?PageNumber=" + pageNumber + "&PageSize=" + pageSize

    if (id) {
        url += "&Id=" + id
    }
    if (ownerId) {
        url += "&OwnerId=" + ownerId
    }
    if (capacity) {
        url += "&Capacity=" + capacity
    }
    const data = await customFetch(url)
    if (typeof (data) === "object") {
        return (data)
    } else {
        console.log("Error:", data)
    }
});

const getSalesContracts = (async (pageNumber, pageSize = 7, id = undefined, ownerId = undefined, capacity = undefined) => {
    let url = baseURL + "SaleContracts?PageNumber=" + pageNumber + "&PageSize=" + pageSize
    if (id) {
        url += "&Id=" + id
    }
    if (ownerId) {
        url += "&OwnerId=" + ownerId
    }
    if (capacity) {
        url += "&Capacity=" + capacity
    }
    const data = await customFetch(url)
    if (typeof (data) === "object") {
        return (data)
    } else {
        console.log("Error:", data)
    }
});

const getRentalContracts = (async (pageNumber, pageSize = 7, id = undefined, PropertyId = undefined, capacity = undefined) => {
    let url = baseURL + "RentalContracts?PageNumber=" + pageNumber + "&PageSize=" + pageSize
    if (id) {
        url += "&Id=" + id
    }
    if (PropertyId) {
        url += "&PropertyId=" + PropertyId
    }
    if (capacity) {
        url += "&Capacity=" + capacity
    }
    const data = await customFetch(url)
    if (typeof (data) === "object") {
        return (data)
    } else {
        console.log("Error:", data)
    }

});

module.exports = { getProperties, getSalesContracts, getRentalContracts }