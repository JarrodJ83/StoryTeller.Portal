import React, { Component } from 'react';
import logo from './logo.svg';
import './App.css';
import RunFeed from './RunFeed';
import RunChart from './RunChart';
import SpecChart from './SpecChart';
import NavHeader from './NavHeader';
import { Button, Grid, Row, Col } from 'react-bootstrap';
class App extends Component {
  render() {
    return (
        <div class="container">
            <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/latest/css/bootstrap.min.css" />
            <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/latest/css/bootstrap-theme.min.css" />
            <Grid>
                <Row className="show-grid">
                    <Col>
                        <NavHeader />
                    </Col>
                </Row>
                <Row className="show-grid">
                    <Col><RunChart /></Col>
                </Row>
                <Row className="show-grid">
                    <Col><RunFeed /></Col>
                </Row>
            </Grid>
      </div>
    );
  }
}

export default App;
