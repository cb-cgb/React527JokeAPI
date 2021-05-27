import React from 'react';
import axios from 'axios';
import {produce} from 'immer';

class Signup extends React.Component {

    state =  {  
        user: {
            name: '',
            email: '',
            passwordhash: ''
        }

    }

    onTextChange = e => {
     const nextState = produce(this.state, draft => {
         draft.user[e.target.name] = e.target.value;
     })
      this.setState(nextState);
    }

    onSubmitForm  = async(e) => {
        e.preventDefault();
        await axios.post('/api/account/signup', this.state.user)
        this.props.history.push('/login');
    }

    render() { 
        return ( 
          
            <div className="row card card-body bg-light" style={{width:400}}>

              <h3 style={{textAlign:"center"}}>Sign Up</h3>
              <form onSubmit={this.onSubmitForm}>
                <input type="text" className="form-control" name="name" onChange={this.onTextChange} placeholder="Enter your name" />
                <input type="text" className="form-control" name="email" onChange={this.onTextChange} placeholder="Enter your email" />
                <input type="password" className="form-control" name="password" onChange={this.onTextChange}  placeholder="Enter your password" />
                <br/>
                <button className="btn btn-lg btn-info">Sign Up</button>
              </form>
          </div> 
           

         );
    }
}
 
export default Signup;