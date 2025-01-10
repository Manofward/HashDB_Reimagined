export function Get()
{
    return document.cookie;
}

export function Set(key, value)
{
    document.cookie = `${key}=${value}`;
}

export function Delete(key)
{
    document.cookie = `${key}=false`;
}