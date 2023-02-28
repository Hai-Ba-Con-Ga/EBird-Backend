import React from "react";
import { Prism as SyntaxHighlighter } from "react-syntax-highlighter";
type Props = {
  results: any[] ;
};
import { isJsonParsable } from "./App";
const JsonBeautify = ({ results }: Props) => {
  if (results) {
    console.log(results);
      return (
        <>
        {results.map((result, i) => (
            <div>
              <h2>{`Param ${i + 1}`}</h2>
              <SyntaxHighlighter
                language="javascript"
                wrapLines={true}
              >{
                // isJsonParsable(result) ? 
                JSON.stringify(result, null, 2)
                // : result
                }</SyntaxHighlighter>
            </div>
          ))}
          {/* {results.map((result, i) => (
            <div>
              <h1>{`Param ${i + 1}`}</h1>
              <SyntaxHighlighter
                language="json"
                wrapLines={true}
              >{
                isJsonParsable(result) ? 
                JSON.stringify(JSON.parse(result), null, 2)
                : result
                }</SyntaxHighlighter>
            </div>
          ))} */}
        </>
      );
    
    }
    else{
        return <></>
    }
  }

export default JsonBeautify;
