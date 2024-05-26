
export const backend = {
  GET: ({url, headers = {}}) =>
    fetch(url, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        ...headers
      }
    })
    .then(response => {
      if (response?.ok) return response.json();
      throw response;
    }),
  POST: ({url, body = {}, headers = {}}) =>
    fetch(url, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        ...headers
      },
      body: JSON.stringify(body)
    })
    .then(response => {
      if (response?.ok) return response.json();
      throw response;
    }),
  PUT: ({url, body = {}, headers = {}}) =>
    fetch(url, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        ...headers
      },
      body: JSON.stringify(body)
    })
    .then(response => {
      console.log(response);
      if (response?.ok) return response.json(); // TODO: Handle empty body response
      throw response;
    })
}