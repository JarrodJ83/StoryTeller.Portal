import React from 'react';
import dotnetify from 'dotnetify';
import { Table, Glyphicon } from 'react-bootstrap';

class RunFeedRow extends React.Component {
    constructor(props) {
        super(props);
        this.state = { Run: this.props.Run };
    }
    componentWillUnmount() {
        this.vm.$destroy();
    }
    render() {
        return (<tr key={this.state.Run.Id}>
            <td>{this.state.Run.Name}</td>
                    <td sytle="color: green;">{this.state.Run.AppName}</td>
                    <td><span>(Pass: {this.state.Run.SuccessfulCount} </span><span sytle="color: green;">Fail: {this.state.Run.FailureCount} Total: {this.state.Run.TotalCount})</span></td>

                    <td>
                        <a target="_blank" href={"http://localhost:1881/Runs/" + this.state.Run.Id + "/results"} style={{ visibility: this.state.Run.Finished ? "visible" : "hidden" }} >View Results</a>
                    </td>
                    <td><Glyphicon glyph={this.state.Run.Finished ? this.state.Run.Passed ? "ok" : "exclamation-sign" : "hourglass"} style={{ color: this.state.Run.Finished ? this.state.Run.Passed ? "green" : "red" : "black" }} /></td>
                </tr >);
    }
}
export default RunFeedRow;