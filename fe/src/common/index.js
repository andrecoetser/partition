export default {
    objectToQuerystring: function (obj) {
        var first = true,          
            qs = ''

        for (var property in obj) {
            if (obj.hasOwnProperty(property)) {                
                var delimiter = first ? '?' : '&';
                property = encodeURIComponent(property);
                var val = encodeURIComponent(obj[property]);
                qs =  [qs, delimiter, property, '=', val].join('');
                first = false;
            }
        }
        return qs;
    },
    years: [2009, 2010, 2011, 2012, 2013, 2014, 2015, 2016],
    months: [{ id: 0, value: 'January'}, 
            { id: 1, value: 'February'}, 
            { id: 2, value: 'March'}, 
            { id: 3, value: 'April'}, 
            { id: 4, value: 'May'}, 
            { id: 5, value: 'June'}, 
            { id: 6, value: 'July'}, 
            { id: 7, value: 'August'}, 
            { id: 8, value: 'September'}, 
            { id: 9, value: 'October'}, 
            { id: 10, value: 'November'}, 
            { id: 11, value: 'December'}
            ],            
    url: '/api/Query/',
    dateFormat: 'DD MMM YYYY',
    timeRange: 'TimeRange',
    year: 'Year',
    month: 'Month',
    all: 'All',
    one: 'One',
    product: 'Product',
    store: 'Store',
    productStore: 'ProductStore',
    productCount: 500000,
    storeCount: 200000
}

 