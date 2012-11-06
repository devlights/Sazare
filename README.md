## サンプルの実行方法

***

サンプルは、MSBuildでビルドし実行できるようになっています。  
実行する際、ビルドして出来上がったEXEに対して引数でクラス名を渡すと  
そのクラスの処理が実行されます。  

実行したいクラスが「XXClass」の場合は以下のようにします。  
> msbuild /t:Build,Run /p:Configuration=Debug /p:Platform=x86 /p:TargetClass=XXClass

普段、私はこのサンプルをVisualStudioではなくテキストエディタで記述してビルド・実行しています。  
みなさんがお使いのテキストエディタでも、外部コマンド機能がついていると思います。  
後は、エディタの外部コマンドにMSBuildを実行するための設定を行えば完了です。  

私は普段 EmEditor を利用しているので以下の設定を登録して利用しています。  
> msbuild /t:Build,Run /p:Configuration=Debug /p:Platform=x86 /p:TargetClass=$(CurText)

上記の設定だと、EmEditorにて現在カーソルがある部分のクラスが実行されるようにできます。
結果をアウトプットバーに表示するようにして、アウトライン機能をONするといい感じになります。


