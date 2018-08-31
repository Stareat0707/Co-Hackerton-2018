using IronPython.Hosting;
using IronPython.Runtime;
using IronPython.Runtime.Exceptions;
using Microsoft.Scripting;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Interpreter : MonoBehaviour
{
    [SerializeField] InputField Editor;
    [SerializeField] Text CompileErrorMessage;
    [SerializeField] GameObject GameManager;
    GameObject character;

    public void Run()
    {
        CompileErrorMessage.text = "";

        File.Copy("Assets/basicCode.py", "Assets/userCode.py", true);
        StreamWriter writer = new StreamWriter("Assets/userCode.py", true);
        writer.WriteLine(Editor.text);
        writer.Close();

        var engine = Python.CreateEngine();
        engine.Runtime.LoadAssembly(Assembly.GetAssembly(typeof(GameObject)));
        var scope = engine.CreateScope();
        var source = engine.CreateScriptSourceFromFile("Assets/userCode.py");
        scope.SetVariable("gameManager", GameManager);

        try
        {
            Character.instance.ReStart();
            source.Execute(scope);
            CompileErrorMessage.text = "컴파일 성공!";
            character = scope.GetVariable("character");
            character.GetComponent<Character>().Action();
        }
        catch (SyntaxErrorException)
        {
            CompileErrorMessage.text =
                 "문제가 생겼어요! 다음을 확인해주세요.\n" +
                 "1. 반복문을 사용할 때  ':'를 빠트리지 않으셨나요?\n" +
                 "2. 들여쓰기가 잘못된 부분이 있나요?";
        }
        catch (UnboundNameException)
        {
            CompileErrorMessage.text =
                 "문제가 생겼어요! 다음을 확인해주세요.\n" +
                 "1. 캐릭터를 움직이기 전 생성을 하셨나요?\n" +
                 "2. 함수 이름이나 매개변수에 오타가 있진 않나요?";
        }
        catch (ValueErrorException)
        {
            CompileErrorMessage.text =
                 "문제가 생겼어요! 다음을 확인해주세요.\n" +
                 "1. 매개 변수에 오타가 있진 않나요?";
        }
        catch (MissingMemberException)
        {
            CompileErrorMessage.text =
                 "문제가 생겼어요! 다음을 확인해주세요.\n" +
                 "1. 코드를 작성해 주세요.";
        }
    }

    public void Stop()
    {
        Character.instance.ReStart();
    }
}
