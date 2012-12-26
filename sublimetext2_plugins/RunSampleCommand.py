import sublime
import sublime_plugin
import os
import subprocess

class RunSampleCommand(sublime_plugin.TextCommand):
  def run(self, edit):
    className = self.view.substr(self.view.sel()[0])
    subprocess.Popen(["cmd", "/k", "msbuild /p:TargetClass=" + className], cwd=os.path.split(self.view.file_name())[0])
