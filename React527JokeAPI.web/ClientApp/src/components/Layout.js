import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { AuthContext } from '../AuthContext';

class Layout extends React.Component {

  render() {
    return (
      <AuthContext.Consumer>
        { value => {
          const { user } = value;
          const userLoggedIn = user;

          return (
            <>
              <nav className="navbar navbar-expand-md navbar-dark bg-dark fixed-top">
                <Link to='/' className="navbar-brand">Jokes</Link>

                <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarsExampleDefault" aria-controls="navbarsExampleDefault" aria-expanded="false" aria-label="Toggle navigation">
                  <span className="navbar-toggler-icon"></span>
                </button>

                <div className="collapse navbar-collapse" id="navbarsExampleDefault">
                  <ul className="navbar-nav mr-auto">
                    <li className="nav-item active"><Link to='/' className="nav-link">Home</Link> </li>
                    {!userLoggedIn && <li className="nav-item active"><Link to='/signup' className="nav-link">Signup</Link> </li>}
                    {!userLoggedIn && <li className="nav-item active"><Link to='/login' className="nav-link">Login</Link> </li> }
                    <li className="nav-item active"><Link to='/alljokes' className="nav-link">All Jokes</Link> </li>
                    {userLoggedIn && <li className="nav-item active"><Link to='/logout' className="nav-link">Logout</Link> </li> }


                  </ul>

                </div>
              </nav>

              <div className="container" style={{ marginTop: 60 }}>
                {this.props.children}
              </div>

            </>
          )}}
      
       </AuthContext.Consumer>
    );
  }
}
export default Layout;