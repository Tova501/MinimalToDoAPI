import axios from "axios";

let myAxios = axios.create({
    baseURL: "https://localhost:5138",
})

myAxios.interceptors.response.use(
    function(response) {
      return response;
    },
   async function(error) {
    console.log(error);
    }
  );

export default myAxios