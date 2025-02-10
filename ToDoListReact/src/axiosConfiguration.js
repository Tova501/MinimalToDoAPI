import axios from "axios";

let myAxios = axios.create({
    baseURL: process.env.REACT_APP_API,
})

myAxios.interceptors.response.use(
    function(response) {
      return response;
    },
   async function(error) {
    console.log(error);
    }
  );

export default myAxiosS