const getUsername=(()=>{
    const token=localStorage.getItem("Token");
    const parts = token.split('.');

    if (parts.length !== 3) {
        throw new Error('Invalid JWT format');
    }

    const payload = atob(parts[1].replace(/-/g, '+').replace(/_/g, '/'));
    const payloadObject = JSON.parse(payload);
    return(payloadObject["cognito:username"])
})


module.exports={getUsername}