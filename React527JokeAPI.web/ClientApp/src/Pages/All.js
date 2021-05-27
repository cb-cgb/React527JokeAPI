import React, { Component } from 'react';
import axios from 'axios';
import { isDraft } from 'immer';

class Home extends Component {
  state = {  

  jokeswithcounts: [{
    jokeId: '',
    question: '',
    punchline: '',
    countlike: 0,
    countdislike: 0
  }]  
  }

  componentDidMount =async () => {
   const {data} = await axios.get('/api/joke/getall'); //get new joke
   await this.setState({jokeswithcounts: data})
    console.log(this.state);
 }

    
  render() { 
    
    const {jokeId,question, punchline,countlike,countdislike} = this.state.jokeswithcounts;

    return ( 
      <>
      {this.state.jokeswithcounts.map(j=> 
        
        <div key={j.jokeId} className="row card card-body bg-light" style={{width: 500, textAlign: 'center'}} >
        
         <span style={{color: 'blueviolet'}}>{j.question}</span>
         <br/>
         <span  style={{color: 'purple'}}>{j.punchline}</span>
         <br/>
         <div className = "row" >
          <div className = "col-md-6">
            <span style={{color: 'lightgreen'}}>#Likes: {j.countLike}</span>
          </div>
          <div className = "col-md-6">
           <span style={{color: 'red'}}>#Not Likes: {j.countDislike}</span>
          </div>
 
         </div>
        </div> )}           {/* //end map */}

      </>
    );
        

  }
}
 
export default Home ;

