import React, { Component } from 'react';
import axios from 'axios';
import { isDraft } from 'immer';

class Home extends Component {
  state = {  

   joke: {
     id:'',
     question: '',
     punchline: ''            
   },
   jokelike: {
     jokeId:'',
     userId: '',
     like: '',
     date: ''
   },
   enableLike:true ,
   enableNotLike:true ,
   isloading: true
         
  }

  componentDidMount =async () => {
   const {data} = await axios.get('/api/joke/getjoke'); //get new joke
   await this.setState({joke: data})
   await this.setState({isloading: false});
   await this.enableDisableLike(); //present the like/dislike buttons correctly upon loading
   console.log(this.state);
 }

  //pull the current like status. this is used to set like/dislike correctly. 
  getLikes = async (jokeId)=> {
    const {data} = await axios.get(`/api/joke/getuserlike?jokeId=${jokeId}`)
    console.log(data);
    await this.setState({jokelike:data});
  }
 
  //check if within window to change
  canChangeLikeStatus =async() => {
    console.log('running canchangeLikeStatus......!');
    const {data} =await axios.get(`/api/joke/withinchangewindow?jokeId=${this.state.joke.id}`);
    console.log(data);
   
    if(!data) { //if not within window to change status, disable both
      console.log('disabling, out of change window or no user logged in')
      this.setState({enableLike: false, enableNotLike: false});
    }
}

  //set the like/dislike state props. 
  enableDisableLike =async() => {
    const {id} = this.state.joke;
    console.log(id);
    await this.getLikes(id);
    const { userId,like, date} = this.state.jokelike;
    console.log(userId);
   
    if (!userId)  {
      console.log('user never liked yet')
      this.setState({enableLike: true, enableNotLike: true}); //if no like exists yet for this user/joke, enable both buttons
    }
    
    else if (like) {
      console.log('liked in the past')
      this.setState({enableLike: !like, enableNotLike: like}); //if previously liked, disable the like, enable dislike
    }
    else if (!like) {
      console.log('disliked in the past')
      this.setState({enableLike: true, enableNotLike: false}); //if previously not liked, disable the dislike, enable like
    }
    await this.canChangeLikeStatus(id); //disable if out of window change
    console.log(this.state.enableLike);
    console.log(this.state.enableNotLike);
  }


   onClickLike =async() => {
    const {id} = this.state.joke;
    await axios.post('/api/joke/likejoke', {JokeId: id,  like:true });
    this.enableDisableLike(); //enable/disable the like/not like based on the latest like action. 
   }

   onClickNotLike =async() => {
    const {id} = this.state.joke;
    await axios.post('/api/joke/likejoke', {jokeid: id, like:false});
    this.enableDisableLike(); //enable/disable the like/not like based on the latest like action.  
   }

  
  render() { 
    
    const {question, punchline} = this.state.joke;

    return ( 
    <div>
      {this.state.isloading && <h3>Loading...</h3>}    
      {!this.state.isloading &&
       <div className="row card card-body bg-light" style={{width: 500, textAlign: 'center'}} >
         <h2 style={{textAlign:"center" }}> Joke</h2>

         <span style={{color: 'blueviolet'}}>{question}</span>
         <br/>
         <span  style={{color: 'purple'}}>{punchline}</span>
         <br/>
         <div className = "row" >
          <div className = "col-md-6">
           <button disabled = {!this.state.enableLike} className="btn btn-success" onClick={this.onClickLike}>Like!</button>
          </div>
          <div className = "col-md-6">
           <button disabled = {!this.state.enableNotLike} className="btn btn-danger" onClick={this.onClickNotLike}>Don't Like</button>
          </div>
         </div>
         
          <div >
           <span style={{width:100, float: 'right'}}className = "btn text-info"  onClick={this.componentDidMount}>Next</span>
          </div>
         
      </div>}

      </div>
      
    );
        

  }
}
 
export default Home ;

