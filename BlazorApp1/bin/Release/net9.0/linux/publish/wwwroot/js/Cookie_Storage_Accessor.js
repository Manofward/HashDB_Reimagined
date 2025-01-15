// This function gets the Cookie
export function Get()
{
    return document.cookie;
}

// This function will be used by the Cookie_Storage_Accessor.cs to set a cookie
export function Set(key, value)
{
    document.cookie = `${key}=${value}`;
}

// This Function will be used from the Cookie_Storage_Accessor.cs to delete the cookie
export function Delete(key)
{
    document.cookie = `${key}=false`;
}