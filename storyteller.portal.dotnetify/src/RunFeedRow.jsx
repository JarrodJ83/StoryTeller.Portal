import React from 'react';
import dotnetify from 'dotnetify';
import { Table, Glyphicon } from 'react-bootstrap';

class RunFeedRow extends React.Component  {
    constructor(props) {
        super(props);
    } 
    shouldComponentUpdate(nextProps) {
        var shouldUpdate = nextProps.Run.FailureCount !== this.props.Run.FailureCount ||
            nextProps.Run.SuccessfulCount !== this.props.Run.SuccessfulCount ||
            nextProps.Run.Finished !== this.props.Run.Finished;

        console.log("ShouldUpdate: " + shouldUpdate);

        return shouldUpdate;
    }
    render() {
        return (<tr key={this.props.Run.Id}>
            <td>{this.props.Run.Name}</td>
            <td style={{ color: "green" }}>{this.props.Run.AppName}</td>
            <td><span>(Pass: {this.props.Run.SuccessfulCount} </span><span style={{ color: "green" }}>Fail: {this.props.Run.FailureCount} Total: {this.props.Run.TotalCount})</span></td>

            <td>
                <a target="_blank" href={"http://localhost:1881/Runs/" + this.props.Run.Id + "/results"} style={{ visibility: this.props.Run.Finished ? "visible" : "hidden" }} >View Results</a>
            </td>
            <td><Glyphicon glyph={this.props.Run.Finished ? this.props.Run.Passed ? "ok" : "exclamation-sign" : "hourglass"} style={{ color: this.props.Run.Finished ? this.props.Run.Passed ? "green" : "red" : "black" }} /></td>
        </tr >);
    }
}
export default RunFeedRow;