import axios from 'axios';

axios.defaults.baseURL = "http://localhost:5138";

export default{
  getTasks: async () => {
    await axios.get(`/items`).then(result=>{
      console.log(result)
      return result.data;
    }).catch(e=>{
      console.log(e);
    })
  },

  addTask: async(name)=>{
    console.log('addTask', name)
    //TODO
    return {};
  },

  setCompleted: async(id, isComplete)=>{
    console.log('setCompleted', {id, isComplete})
    //TODO
    return {};
  },

  deleteTask:async()=>{
    console.log('deleteTask')
  }
};
