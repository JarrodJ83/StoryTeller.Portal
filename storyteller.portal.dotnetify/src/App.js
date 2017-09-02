import React, { Component } from 'react';
import logo from './logo.svg';
import './App.css';
import RunFeed from './RunFeed';
import RunChart from './RunChart';
import NavHeader from './NavHeader';
import { Button, Grid, Row, Col, Navbar, Nav, NavItem, NavDropdown, MenuItem, DropdownMenu } from 'react-bootstrap';
class App extends Component {
  render() {
    return (
        <div>
            <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/latest/css/bootstrap.min.css" />
            <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/latest/css/bootstrap-theme.min.css" />
            <Grid>
                <Row>
                    <Col>
                       <NavHeader />
                    </Col>
                </Row>
                <Row>
                    <Col><RunChart /></Col>
                </Row>
                <Row>
                    <Col><RunFeed /></Col>
                </Row>
            </Grid>
      </div>
    );
  }
}

export default App;
