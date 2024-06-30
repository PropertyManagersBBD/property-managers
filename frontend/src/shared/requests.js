const baseURL="https://localhost:7147/PropertyManager/"


const getProperties=(async (pageNumber) =>{
    try{
        const response=await fetch(baseURL+"Properties/"+pageNumber)
        const data=await response.json()
        return(data)
    }catch(e){
        console.log("Error could not get data")
    }
});

const getSalesContracts=(async (pageNumber) =>{
    try{

        const response=await fetch(baseURL+"SaleContracts/"+pageNumber)
        const data=await response.json()
        return(data)
    }catch(e){
        console.log("Error could not get data")
    }
});

const getRentalContracts=(async (pageNumber) =>{
    try{
        
        const response=await fetch(baseURL+"RentalContracts/"+pageNumber)
        const data=await response.json()
        return(data)
    }catch(e){
        console.log("Error could not get data")
    }
});

const verifyJWT =(async (idToken, accessToken) =>{
    try{
        const response= await fetch(baseURL+"verifyJWT?idToken="+idToken+"&accessToken="+accessToken)
        const data=await response.json();
        return(data)
    }catch(e){
        console.log("Failed to verify token")
    }
})
module.exports={getProperties,getSalesContracts,getRentalContracts,verifyJWT}