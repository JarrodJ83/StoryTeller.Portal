import React, { Component } from 'react';
import logo from './logo.svg';
import './App.css';
import RunFeed from './RunFeed';

class App extends Component {
  render() {
    return (
      <div className="App">
        <div className="App-header">
          <img src={logo} className="App-logo" alt="logo" />
          <h2>StoryTeller Portal</h2>
        </div>
          <div id="container">
                <div id="nav">
                    Home<br />
                    Apps
                </div>
                <div id="main"><RunFeed /></div>
          </div>
      </div>
    );
  }
}

export default App;
